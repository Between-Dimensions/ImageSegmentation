using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ImageTemplate
{
    public partial class MainApplication : Form
    {
        private enum ViewMode
        {
            Orignal_Image,
            Gauss_Filter,
            Solid_Color,
            Blend_Image,
        }

        private Dictionary<ViewMode, string> _viewModeStr = new Dictionary<ViewMode, string>
        {
            { ViewMode.Orignal_Image, "Original Image" },
            { ViewMode.Gauss_Filter, "Gauss Filter" },
            { ViewMode.Solid_Color, "Solid Color" },
            { ViewMode.Blend_Image, "Blend Image" },
        };

        private Dictionary<ViewMode, ColorChannel> _viewModeChannel = new Dictionary<ViewMode, ColorChannel>()
        {
            { ViewMode.Orignal_Image, ColorChannel.All },
            { ViewMode.Gauss_Filter, ColorChannel.All },
            { ViewMode.Solid_Color, ColorChannel.All },
            { ViewMode.Blend_Image, ColorChannel.All },
        };

        private RGBPixel[,] _originalImage;
        private RGBPixel[,] _solidColorSegmentedImage;

        private int[] _currentSegments;
        private int _imageWidth, _imageHeight;
        private HashSet<int> _selectedSegments = new HashSet<int>();

        private ComboBox[] _comboViews = new ComboBox[2];
        private PictureBox[] _renderPanels = new PictureBox[2];

        public MainApplication()
        {
            InitializeComponent();
        }

        private void MainApplication_Load(object sender, EventArgs e)
        {
            nudMaskSize.Enabled = isGauss.Checked;
            txtGaussSigma.Enabled = isGauss.Checked;

            _comboViews[0] = cboxView1;
            _comboViews[1] = cboxView2;

            _renderPanels[0] = pictureBox1;
            _renderPanels[1] = pictureBox2;

            foreach (ComboBox comboBox in _comboViews)
            {
                comboBox.DisplayMember = "Text";
                comboBox.ValueMember = "Value";

                foreach (ViewMode mode in _viewModeStr.Keys)
                    comboBox.Items.Add(new { Text = _viewModeStr[mode], Value = mode });
            }

            _comboViews[0].SelectedIndex = (int)ViewMode.Orignal_Image;
            _comboViews[1].SelectedIndex = (int)ViewMode.Gauss_Filter;
        }

        private void isGauss_CheckedChanged(object sender, EventArgs e)
        {
            nudMaskSize.Enabled = isGauss.Checked;
            txtGaussSigma.Enabled = isGauss.Checked;
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            tboxFilePath.Clear();
            tboxSize.Clear();

            foreach (PictureBox pictureBox in _renderPanels)
                pictureBox.Image = null;

            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //Open the browsed image and display it
                string OpenedFilePath = openFileDialog1.FileName;
                _originalImage = ImageOperations.OpenImage(OpenedFilePath);
                ImageOperations.DisplayImage(_originalImage, pictureBox1);

                string width = ImageOperations.GetWidth(_originalImage).ToString();
                string height = ImageOperations.GetHeight(_originalImage).ToString();
                tboxSize.Text = $"{width} x {height}";
                tboxFilePath.Text = OpenedFilePath;
            }
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            double sigma = double.Parse(txtGaussSigma.Text);
            int maskSize = (int)nudMaskSize.Value;
            float k = float.Parse(tboxK.Text);
            float blend = (float)(numericBlend.Value / numericBlend.Maximum);

            _imageWidth = ImageOperations.GetWidth(_originalImage);
            _imageHeight = ImageOperations.GetHeight(_originalImage);

            RGBPixel[,] gaussImage = ImageOperations.GaussianFilter1D(_originalImage, maskSize, sigma);

            Stopwatch timer = Stopwatch.StartNew();
            int[] segments = PixelGraphSegmentator.Segment(isGauss.Checked ? gaussImage : _originalImage, k);
            timer.Stop();
            tboxElapsedTime.Text = timer.ElapsedMilliseconds.ToString();
            _currentSegments = segments;
            _solidColorSegmentedImage = SolidColorImage(segments, _imageWidth, _imageHeight);
            _selectedSegments.Clear();

            for (int i = 0; i < _renderPanels.Length; i++)
            {
                RGBPixel[,] renderedImage = null;
                ViewMode mode = (ViewMode)_comboViews[i].SelectedIndex;
                ColorChannel channel = _viewModeChannel[mode];

                switch (mode)
                {
                    case ViewMode.Orignal_Image:
                        renderedImage = _originalImage;
                        break;
                    case ViewMode.Gauss_Filter:
                        renderedImage = gaussImage;
                        break;
                    case ViewMode.Solid_Color:
                        renderedImage = _solidColorSegmentedImage;
                        break;
                    case ViewMode.Blend_Image:
                        renderedImage = BlendImage(_originalImage, _solidColorSegmentedImage, blend, channel);
                        break;
                }

                ImageOperations.DisplayImage(renderedImage, _renderPanels[i]);
            }

            WriteSegements(segments);
        }

        private RGBPixel[,] SolidColorImage(int[] segments, int width, int height)
        {
            RGBPixel[,] outImage = new RGBPixel[height, width];
            Dictionary<int, Color> colorMap = new Dictionary<int, Color>();
            Random rand = new Random();

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int segment = segments[y * width + x];

                    if (!colorMap.TryGetValue(segment, out Color segmentColor))
                    {
                        segmentColor = Color.FromArgb(
                            rand.Next(50, 255),  // Red
                            rand.Next(50, 255),  // Green
                            rand.Next(50, 255)   // Blue
                        );
                        colorMap[segment] = segmentColor;
                    }

                    outImage[y, x].red = segmentColor.R;
                    outImage[y, x].green = segmentColor.G;
                    outImage[y, x].blue = segmentColor.B;
                }
            }

            return outImage;
        }

        private RGBPixel[,] BlendImage(RGBPixel[,] a, RGBPixel[,] b, float alpha, ColorChannel channel)
        {
            int width = ImageOperations.GetWidth(_originalImage);
            int height = ImageOperations.GetHeight(_originalImage);
            RGBPixel[,] outImage = new RGBPixel[height, width];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    switch (channel)
                    {
                        case ColorChannel.Red:
                            outImage[y, x].red = (byte)Lerp(a[y, x].red, b[y, x].red, alpha);
                            break;
                        case ColorChannel.Green:
                            outImage[y, x].green = (byte)Lerp(a[y, x].green, b[y, x].green, alpha);
                            break;
                        case ColorChannel.Blue:
                            outImage[y, x].blue = (byte)Lerp(a[y, x].blue, b[y, x].blue, alpha);
                            break;
                        case ColorChannel.All:
                            outImage[y, x].red = (byte)Lerp(a[y, x].red, b[y, x].red, alpha);
                            outImage[y, x].green = (byte)Lerp(a[y, x].green, b[y, x].green, alpha);
                            outImage[y, x].blue = (byte)Lerp(a[y, x].blue, b[y, x].blue, alpha);
                            break;
                    }
                }
            }

            return outImage;
        }

        public static float Lerp(float a, float b, float t)
        {
            return a + (b - a) * t;
        }

        public static void WriteSegements(int[] segments)
        {
            Dictionary<int, int> pixelsPerSegment = new Dictionary<int, int>();

            for (int i = 0; i < segments.Length; i++)
                pixelsPerSegment[segments[i]] = 0;

            for (int i = 0; i < segments.Length; i++)
                pixelsPerSegment[segments[i]]++;

            var sortedSegments = pixelsPerSegment.OrderByDescending(x => x.Value);
            using (StreamWriter outputFile = new StreamWriter("output.txt"))
            {
                outputFile.WriteLine(pixelsPerSegment.Count);
                foreach (var seg in sortedSegments)
                    outputFile.WriteLine(seg.Value);
            }
        }

        private void selectedIndexChanged(object sender, EventArgs e)
        {
            if (_originalImage == null || _solidColorSegmentedImage == null) return;

            double sigma = double.Parse(txtGaussSigma.Text);
            int maskSize = (int)nudMaskSize.Value;
            float blend = (float)(numericBlend.Value / numericBlend.Maximum);

            for (int i = 0; i < _renderPanels.Length; i++)
            {
                RGBPixel[,] renderedImage = null;
                ViewMode mode = (ViewMode)_comboViews[i].SelectedIndex;
                ColorChannel channel = _viewModeChannel[mode];

                switch (mode)
                {
                    case ViewMode.Orignal_Image:
                        renderedImage = _originalImage;
                        break;
                    case ViewMode.Gauss_Filter:
                        RGBPixel[,] gaussImage = ImageOperations.GaussianFilter1D(_originalImage, maskSize, sigma);
                        renderedImage = gaussImage;
                        break;
                    case ViewMode.Solid_Color:
                        renderedImage = _solidColorSegmentedImage;
                        break;
                    case ViewMode.Blend_Image:
                        renderedImage = BlendImage(_originalImage, _solidColorSegmentedImage, blend, channel);
                        break;
                }

                ImageOperations.DisplayImage(renderedImage, _renderPanels[i]);
            }
        }

        private void btnMergeRegions_Click(object sender, EventArgs e)
        {
            if (_selectedSegments.Count < 2 || _currentSegments == null) return;

            int mergeLabel = _selectedSegments.Min();
            for (int i = 0; i < _currentSegments.Length; i++)
            {
                if (_selectedSegments.Contains(_currentSegments[i]))
                    _currentSegments[i] = mergeLabel;
            }
            
            _solidColorSegmentedImage = SolidColorImage(_currentSegments, _imageWidth, _imageHeight);
            _selectedSegments.Clear();
            HighlightSelectedRegions();
        }

        private void pictureBox2_MouseClick(object sender, MouseEventArgs e)
        {
            if (_currentSegments == null || _imageWidth == 0 || _imageHeight == 0) return;
            var pb = (PictureBox)sender;
            if (pb.Image == null) return;
            int imgX = e.X, imgY = e.Y;

            if (imgX < 0 || imgX >= _imageWidth || imgY < 0 || imgY >= _imageHeight) return;

            int idx = imgY * _imageWidth + imgX;
            if (idx < 0 || idx >= _currentSegments.Length) return;
            int segLabel = _currentSegments[idx];

            if (_selectedSegments.Contains(segLabel))
                _selectedSegments.Remove(segLabel);
            else
                _selectedSegments.Add(segLabel);

            HighlightSelectedRegions();
        }

        private void HighlightSelectedRegions()
        {
            if (_solidColorSegmentedImage == null || _currentSegments == null) return;

            //RGBPixel[,] displayImage = new RGBPixel[_imageHeight, _imageWidth];

            //for (int y = 0; y < _imageHeight; y++)
            //    for (int x = 0; x < _imageWidth; x++)
            //        displayImage[y, x] = _solidColorSegmentedImage[y, x];

            RGBPixel[,] displayImage = (RGBPixel[,])_solidColorSegmentedImage.Clone();

            for (int y = 0; y < _imageHeight; y++)
            {
                for (int x = 0; x < _imageWidth; x++)
                {
                    int idx = y * _imageWidth + x;
                    if (_selectedSegments.Contains(_currentSegments[idx]))
                    {
                        displayImage[y, x].red = 255;
                        displayImage[y, x].green = 255;
                        displayImage[y, x].blue = 0;
                    }
                }
            }
            ImageOperations.DisplayImage(displayImage, pictureBox2);
        }
    }
}
