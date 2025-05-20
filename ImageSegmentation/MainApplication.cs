using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Drawing.Imaging;

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
            Selected_Regions,
        }

        private Dictionary<ViewMode, string> _viewModeStr = new Dictionary<ViewMode, string>
        {
            { ViewMode.Orignal_Image, "Original Image" },
            { ViewMode.Gauss_Filter, "Gauss Filter" },
            { ViewMode.Solid_Color, "Solid Color" },
            { ViewMode.Blend_Image, "Blend Image" },
            { ViewMode.Selected_Regions, "Selected Regions" },
        };

        private Dictionary<ViewMode, ColorChannel> _viewModeChannel = new Dictionary<ViewMode, ColorChannel>()
        {
            { ViewMode.Orignal_Image, ColorChannel.All },
            { ViewMode.Gauss_Filter, ColorChannel.All },
            { ViewMode.Solid_Color, ColorChannel.All },
            { ViewMode.Blend_Image, ColorChannel.All },
            { ViewMode.Selected_Regions, ColorChannel.All },
        };

        private RGBPixel[,] _originalImage;
        private RGBPixel[,] _solidColorSegmentedImage;

        private readonly Dictionary<int, Color> _segmentColorMap = new Dictionary<int, Color>();

        private int[] _imageSegments;
        private int _imageWidth, _imageHeight;
        private readonly HashSet<int> _selectedSegments = new HashSet<int>();

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

        private void ClearUI()
        {
            tboxFilePath.Clear();
            tboxSize.Clear();
            tboxElapsedTime.Clear();

            // Not a UI element, but needs be cleared with the UI
            _selectedSegments.Clear();
            _segmentColorMap.Clear();

            foreach (PictureBox pictureBox in _renderPanels)
                pictureBox.Image = null;
        }

        private void isGauss_CheckedChanged(object sender, EventArgs e)
        {
            nudMaskSize.Enabled = isGauss.Checked;
            txtGaussSigma.Enabled = isGauss.Checked;
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                ClearUI();

                //Open the browsed image and display it
                string OpenedFilePath = openFileDialog1.FileName;
                _originalImage = ImageOperations.OpenImage(OpenedFilePath);

                _imageWidth = ImageOperations.GetWidth(_originalImage);
                _imageHeight = ImageOperations.GetHeight(_originalImage);

                ImageOperations.DisplayImage(_originalImage, pictureBox1);

                tboxSize.Text = $"{_imageWidth.ToString()} x {_imageHeight.ToString()}";
                tboxFilePath.Text = OpenedFilePath;

                _imageSegments = null;
            }
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            _selectedSegments.Clear();
            _segmentColorMap.Clear();

            double sigma = double.Parse(txtGaussSigma.Text);
            int maskSize = (int)nudMaskSize.Value;
            int k = int.Parse(tboxK.Text);

            RGBPixel[,] gaussImage = ImageOperations.GaussianFilter1D(_originalImage, maskSize, sigma);
            {
                Stopwatch timer = Stopwatch.StartNew();
                _imageSegments = PixelGraphSegmentator.Segment(isGauss.Checked ? gaussImage : _originalImage, k);
                timer.Stop();

                tboxElapsedTime.Text = timer.ElapsedMilliseconds.ToString();
            }
            _solidColorSegmentedImage = SolidColorImage(_imageSegments, _imageWidth, _imageHeight);

            RefreshRenderPanels();
            WriteSegements(_imageSegments);
        }

        private RGBPixel[,] SolidColorImage(int[] segments, int width, int height)
        {
            RGBPixel[,] outImage = new RGBPixel[height, width];
            Random rand = new Random();

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int segment = segments[y * width + x];

                    if (!_segmentColorMap.TryGetValue(segment, out Color segmentColor))
                    {
                        segmentColor = Color.FromArgb(
                            rand.Next(50, 255),  // Red
                            rand.Next(50, 255),  // Green
                            rand.Next(50, 255)   // Blue
                        );
                        _segmentColorMap[segment] = segmentColor;
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
            RefreshRenderPanels();
        }

        private void btnMergeRegions_Click(object sender, EventArgs e)
        {
            if (_selectedSegments.Count < 2 || _imageSegments == null) return;

            int mergeLabel = _selectedSegments.First();
            for (int i = 0; i < _imageSegments.Length; i++)
            {
                if (_selectedSegments.Contains(_imageSegments[i]))
                    _imageSegments[i] = mergeLabel;
            }

            _solidColorSegmentedImage = SolidColorImage(_imageSegments, _imageWidth, _imageHeight);
            _selectedSegments.Clear();
            //_selectedSegments.Add(mergeLabel);

            RefreshRenderPanels();
        }

        private void btnClearSegments_Click(object sender, EventArgs e)
        {
            _selectedSegments.Clear();
            RefreshRenderPanels();
        }

        private void pictureBox_MouseClick(object sender, MouseEventArgs e)
        {
            if (_imageSegments == null || _imageWidth == 0 || _imageHeight == 0) return;

            int idx = e.Y * _imageWidth + e.X;
            if (idx < 0 || idx >= _imageSegments.Length) return;

            int segLabel = _imageSegments[idx];
            if (_selectedSegments.Contains(segLabel))
                _selectedSegments.Remove(segLabel);
            else
                _selectedSegments.Add(segLabel);

            RefreshRenderPanels();
        }

        private RGBPixel[,] HighlightSelectedRegions(RGBPixel[,] image, bool invert)
        {
            if (_imageSegments == null) return null;

            RGBPixel[,] displayImage = (RGBPixel[,])image.Clone();
            for (int y = 0; y < _imageHeight; y++)
            {
                for (int x = 0; x < _imageWidth; x++)
                {
                    int idx = y * _imageWidth + x;

                    if (invert)
                    {
                        if (!_selectedSegments.Contains(_imageSegments[idx]))
                        {
                            displayImage[y, x].red = 255;
                            displayImage[y, x].green = 255;
                            displayImage[y, x].blue = 255;
                        }
                    }
                    else
                    {
                        if (_selectedSegments.Contains(_imageSegments[idx]))
                        {
                            displayImage[y, x].red = 255;
                            displayImage[y, x].green = 255;
                            displayImage[y, x].blue = 0;
                        }
                    }
                }
            }

            return displayImage;
        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            if(pictureBox1.Image == null || pictureBox2.Image == null)
            {
                MessageBox.Show("No image to Download");
                return;
            }
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "bmp files (*.bmp)|*.bmp|All files (*.*)|*.*";
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    pictureBox2.Image.Save(saveFileDialog1.FileName,ImageFormat.Bmp);
                    MessageBox.Show("Image downloaded");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error couldn't download image: " + ex.Message);
                }
            }

        }

        private void RefreshRenderPanels()
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
                    case ViewMode.Selected_Regions:
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

                renderedImage = HighlightSelectedRegions(renderedImage, mode == ViewMode.Selected_Regions);
                ImageOperations.DisplayImage(renderedImage, _renderPanels[i]);
            }
        }
    }
}
