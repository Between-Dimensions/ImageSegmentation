using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ImageTemplate
{
    public enum ColorChannel { Red, Green, Blue, All }

    public static class PixelGraphSegmentator
    {
        private class Edge
        {
            public int v;
            public int w;
            public int weight;

            public Edge(int v, int w, int weight)
            {
                this.v = v;
                this.w = w;
                this.weight = weight;
            }
        }

        public static int[] Segment(RGBPixel[,] image, float k)
        {
            Task<int[]> redTask = Task <int[]>.Factory.StartNew(() => { return SegmentChannel(image, ColorChannel.Red, k); });
            Task<int[]> greenTask = Task<int[]>.Factory.StartNew(() => { return SegmentChannel(image, ColorChannel.Green, k); });
            Task<int[]> blueTask = Task<int[]>.Factory.StartNew(() => { return SegmentChannel(image, ColorChannel.Blue, k); });
            Task.WaitAll(redTask, greenTask, blueTask);
            
            return MergeSegmentChannels(redTask.Result, greenTask.Result, blueTask.Result);
        }

        public static int[] SegmentChannel(RGBPixel[,] image, ColorChannel channel, float k)
        {
            // Kruskal’s minimum spanning tree (MST) algorithm implementation using UnionFind

            int pixelCount = GetPixelCount(image);
            UnionFind components = new UnionFind(pixelCount);
            int[] segmentMap = new int[pixelCount];

            List<Edge> edges = ConstructEdges(image, channel);
            edges.Sort((a, b) => a.weight.CompareTo(b.weight));

            foreach (Edge e in edges)
            {
                int c1 = components.Find(e.v);
                int c2 = components.Find(e.w);

                if (c1 != c2)
                {
                    float c1InternalDiff = components.InternalDiff(c1);
                    float c2InternalDiff = components.InternalDiff(c2);
                    float minInteralDiff = Math.Min(c1InternalDiff + k / components.Size(c1),
                                                    c2InternalDiff + k / components.Size(c2));

                    // Edge `e` is the minimum weight edge connecting the two components
                    // i.e. `e.weight` is Dif(C1, C2)
                    if (e.weight <= minInteralDiff)
                        components.Union(c1, c2, e.weight);
                }
            }

            for (int i = 0; i < pixelCount; i++)
                segmentMap[i] = components.Find(i);

            return segmentMap;
        }

        private static int[] MergeSegmentChannels(int[] red, int[] green, int[] blue)
        {
            Debug.Assert(red.Length == green.Length && green.Length == blue.Length);

            int pixelCount = red.Length;
            int[] segmentMap = new int[pixelCount];
            var uniqueSegments = new Dictionary<(int, int, int), int>();
            int currentId = 0;

            for (int i = 0; i < pixelCount; i++)
            {
                var key = (red[i], green[i], blue[i]);

                if (!uniqueSegments.TryGetValue(key, out int id))
                {
                    id = currentId;
                    uniqueSegments[key] = currentId;
                    currentId++;
                }

                segmentMap[i] = id;
            }

            return segmentMap;
        }

        private static List<Edge> ConstructEdges(RGBPixel[,] image, ColorChannel channel)
        {
            (int dx, int dy)[] directions = {
                (-1,  0), // Left
                ( 1,  0), // Right
                ( 0,  1), // Up
                ( 0, -1), // Down
                (-1,  1), // Top-Left
                ( 1,  1), // Top-Right
                (-1, -1), // Bottom-Left
                ( 1, -1)  // Bottom-Right
            };

            int width = ImageOperations.GetWidth(image);
            int height = ImageOperations.GetHeight(image);

            // Divide by 2 to avoid double edges
            // `a-b` and `b-a` are the same edge, we don't need to count it twice
            int nEdges = (width * height * 8) / 2;

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

                    foreach ((int dx, int dy) in directions)
                    {
                        int nX = x + dx;
                        int nY = y + dy;
                        bool isOutOfBounds = (Math.Min(nX, nY) < 0) || (nX >= width) || (nY >= height);

                        // Can we do better? Can we create a branch-free loop?
                        if (!isOutOfBounds)
                        {
                            RGBPixel nPixel = image[nY, nX];
                            int nIntensity = GetChannelPixelIntensity(nPixel, channel);
                            int nPixelIndex = To1DIndex(image, nX, nY);

                            if (currPixelIndex < nPixelIndex)
                            {
                                Edge e = new Edge(currPixelIndex, nPixelIndex, Math.Abs(nIntensity - currIntensity));
                                edges.Add(e);
                            }
                        }
                    }
                }
            }

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
