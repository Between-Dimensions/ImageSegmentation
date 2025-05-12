using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace ImageTemplate
{
    public enum ColorChannel { Red, Green, Blue, All }

    public static class PixelGraphSegmentator
    {
        private enum Direction : byte { Left, Right, Top, Bottom, TopLeft, TopRight, BottomLeft, BottomRight }

        // Avoid padding
        // Size should be (6 bytes)
        [StructLayout(LayoutKind.Sequential, Pack = 2)]
        private struct Edge
        {
            private int _v;

            // Optimization:
            // Storing the direction (1 byte) of the W vertex only, essentially halving the size of the Edge struct.
            // This should help with avoiding GC and paging, since we are working on the 3 color channels at once.
            //
            // This possible because all edges like between 2 adj. verticies.
            private Direction _wVertexDirection;

            public byte weight;

            public Edge(int v, Direction direction, byte weight)
            {
                _v = v;
                _wVertexDirection = direction;
                this.weight = weight;
            }

            public int V() => _v;

            // Not the best API design, but it works
            public int W(int width)
            {
                (int dx, int dy) = _directionsMap[_wVertexDirection];
                return _v + (dy * width + dx);
            }
        }

        private static readonly Dictionary<Direction, (int dx, int dy)> _directionsMap = new Dictionary<Direction, (int dx, int dy)> {
            { Direction.Left,        (-1,  0) },
            { Direction.Right,       ( 1,  0) },
            { Direction.Top,         ( 0,  1) },
            { Direction.Bottom,      ( 0, -1) },
            { Direction.TopLeft,     (-1,  1) },
            { Direction.TopRight,    ( 1,  1) },
            { Direction.BottomLeft,  (-1, -1) },
            { Direction.BottomRight, ( 1, -1) },
        };

        public static int[] Segment(RGBPixel[,] image, int k)
        {
            Task<int[]> redTask = Task.Run(() => SegmentChannel(image, ColorChannel.Red, k));
            Task<int[]> greenTask = Task.Run(() => SegmentChannel(image, ColorChannel.Green, k) );
            Task<int[]> blueTask = Task.Run(() => SegmentChannel(image, ColorChannel.Blue, k));
            Task.WaitAll(redTask, greenTask, blueTask);

            int width = ImageOperations.GetWidth(image);
            int height = ImageOperations.GetHeight(image);
            return MergeSegmentChannels(width, height, redTask.Result, greenTask.Result, blueTask.Result);
        }

        public static int[] SegmentChannel(RGBPixel[,] image, ColorChannel channel, int k)
        {
            // Kruskal’s minimum spanning tree (MST) algorithm implementation using UnionFind

            int imageWidth = ImageOperations.GetWidth(image);
            int pixelCount = GetPixelCount(image);
            int[] size = new int[pixelCount];
            // We only care abount the smallest connectivity edge weight and not the whole MST.
            int[] internalDiff = new int[pixelCount];

            UnionFind components = new UnionFind(pixelCount);
            List<Edge> edges = ConstructEdges(image, channel);
            edges.Sort((a, b) => a.weight.CompareTo(b.weight));

            for (int i = 0; i < pixelCount; i++)
                size[i] = 1;

            foreach (Edge e in edges)
            {
                int c1 = components.Find(e.V());
                int c2 = components.Find(e.W(imageWidth));

                if (c1 != c2)
                {
                    int c1InternalDiff = internalDiff[c1];
                    int c2InternalDiff = internalDiff[c2];
                    float minInteralDiff = Math.Min(c1InternalDiff + (float) k / size[c1],
                                                    c2InternalDiff + (float) k / size[c2]);

                    // Edge `e` is the minimum weight edge connecting the two components
                    // i.e. `e.weight` is Dif(C1, C2)
                    if (e.weight <= minInteralDiff)
                    {
                        components.Union(c1, c2);

                        int newInternalDiff = Math.Max(e.weight, Math.Max(c1InternalDiff, c2InternalDiff));
                        internalDiff[c1] = newInternalDiff;
                        internalDiff[c2] = newInternalDiff;

                        int newSize = size[c1] + size[c2];
                        size[c1] = newSize;
                        size[c2] = newSize;
                    }
                }
            }

            return components.FlattenParent();
        }

        private static int[] MergeSegmentChannels(int width, int height, int[] red, int[] green, int[] blue)
        {
            Debug.Assert((red.Length == width * height) && (red.Length == green.Length) && (green.Length == blue.Length));

            int pixelCount = red.Length;
            UnionFind components = new UnionFind(pixelCount);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int currPixelIndex = To1DIndex(x, y, width, height);

                    foreach (var dirction in _directionsMap)
                    {
                        int nX = x + dirction.Value.dx;
                        int nY = y + dirction.Value.dy;
                        bool isOutOfBounds = (Math.Min(nX, nY) < 0) || (nX >= width) || (nY >= height);

                        if (!isOutOfBounds)
                        {
                            int nPixelIndex = To1DIndex(nX, nY, width, height);
                            bool isSameComponent = (red[currPixelIndex] == red[nPixelIndex])     &&
                                                   (green[currPixelIndex] == green[nPixelIndex]) &&
                                                   (blue[currPixelIndex] == blue[nPixelIndex]);

                            if (isSameComponent)
                                components.Union(currPixelIndex, nPixelIndex);
                        }
                    }
                }
            }

            return components.FlattenParent();
        }

        private static List<Edge> ConstructEdges(RGBPixel[,] image, ColorChannel channel)
        {
            int width = ImageOperations.GetWidth(image);
            int height = ImageOperations.GetHeight(image);

            // Divide by 2 to avoid double edges
            // `a-b` and `b-a` are the same edge, we don't need to count it twice
            int nEdges = (((width - 1) * (height - 1) * 8) / 2) + ((width + height) * 2);

            // Optimization:
            // Preallocate the required space in advance to avoid unnecessary vector allocations and copying.
            // This will lead to overallocation, because not all pixels have 8-neighbors, edge and corner pixels, but
            // the extra capacity can be neglected
            List<Edge> edges = new List<Edge>(nEdges);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    RGBPixel currPixel = image[y, x];
                    int currPixelIndex = To1DIndex(image, x, y);
                    int currIntensity = GetChannelPixelIntensity(currPixel, channel);

                    foreach (var direction in _directionsMap)
                    {
                        int nX = x + direction.Value.dx;
                        int nY = y + direction.Value.dy;
                        bool isOutOfBounds = (Math.Min(nX, nY) < 0) || (nX >= width) || (nY >= height);

                        // Can we do better? Can we create a branch-free loop?
                        if (!isOutOfBounds)
                        {
                            RGBPixel nPixel = image[nY, nX];
                            int nIntensity = GetChannelPixelIntensity(nPixel, channel);
                            int nPixelIndex = To1DIndex(image, nX, nY);

                            if (currPixelIndex < nPixelIndex)
                            {
                                Edge e = new Edge(currPixelIndex, direction.Key, (byte) Math.Abs(nIntensity - currIntensity));
                                edges.Add(e);
                            }
                        }
                    }
                }
            }

            Debug.Assert(edges.Count <= nEdges, "Incorrect number of edges calculations!");
            //unsafe
            //{
            //    Console.WriteLine($"Edge Size   : {sizeof(Edge)}B");
            //    Console.WriteLine($"Wasted Space: {(nEdges - edges.Count) * sizeof(Edge) / 1024}KB"); 
            //}

            return edges;
        }

        private static int GetPixelCount(RGBPixel[,] image)
        {
            int width = ImageOperations.GetWidth(image);
            int height = ImageOperations.GetHeight(image);
            return width * height;
        }

        private static int GetChannelPixelIntensity(RGBPixel p, ColorChannel channel)
        {
            switch (channel)
            {
                case ColorChannel.Red:
                    return p.red;
                case ColorChannel.Green:
                    return p.green;
                case ColorChannel.Blue:
                    return p.blue;
            }

            Debug.Assert(false, $"Uknown pixel channel ({channel}).");
            return -1;
        }

        private static int To1DIndex(int x, int y, int width, int height)
        {
            int index = y * width + x;
            Debug.Assert((index >= 0) && (index < width * height));
            return index;
        }

        private static int To1DIndex(RGBPixel[,] image, int x, int y)
        {
            int width = ImageOperations.GetWidth(image);
            int height = ImageOperations.GetHeight(image);
            int index = y * width + x;

            Debug.Assert((index >= 0) && (index < width * height));
            return index;
        }
    }
}
