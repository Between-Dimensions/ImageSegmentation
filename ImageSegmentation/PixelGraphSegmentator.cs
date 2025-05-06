using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ImageTemplate
{
    public static class PixelGraphSegmentator
    {
        public enum ColorChannel { Red, Green, Blue }

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
