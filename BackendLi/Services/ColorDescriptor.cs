using System.Numerics;

namespace BackendLi.Services;
public class ColorDescriptor
{
    private readonly (int, int, int) bins;

    public ColorDescriptor((int, int, int) bins)
    {
        this.bins = bins;
    }

   public float[] DescribeImage(Image<Rgba32> image)
{

 
    int binsH = bins.Item1;
    int binsS = bins.Item2;
    int binsV = bins.Item3;

    List<float[]> featuresList = new List<float[]>();
    image = ConvertImageRgbToHsv(image);

    int width = image.Width;
    int height = image.Height;

    int cx = width / 2;
    int cy = height / 2;

    int[,] segments = { { 0, cx, 0, cy }, { cx, width, 0, cy }, { cx, width, cy, height }, { 0, cx, cy, height } };

    int ex = width * 3 / 8;
    int ey = height * 3 / 8;

    using (var ellipMask = new Image<L8>(width, height))
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (IsInsideEllipse(x, y, cx, cy, ex, ey))
                {
                    ellipMask[x, y] = new L8(255); 
                }
                else
                {
                    ellipMask[x, y] = new L8(0);
                }
            }
        }

        for (int i = 0; i < segments.GetLength(0); i++)
        {
            int startX = segments[i, 0];
            int endX = segments[i, 1];
            int startY = segments[i, 2];
            int endY = segments[i, 3];

            using (var cornerMask = new Image<L8>(width, height))
            {
                for (int y = startY; y < endY; y++)
                {
                    for (int x = startX; x < endX; x++)
                    {
                        if (ellipMask[x, y] == new L8(255))
                        {
                            cornerMask[x, y] = new L8(0); //poate ii 0
                        }
                        else
                        {
                            cornerMask[x, y] = new L8(255); 
                        }
                    }
                }

                float[] hist = CalculateHistogram(image, cornerMask);
                featuresList.Add(hist);
            }
        }

        float[] ellipseHist = CalculateHistogram(image, ellipMask);
        featuresList.Add(ellipseHist);
    }

    float[] meanFeatures = new float[binsH * binsV * binsS];
    int totalCount = featuresList.Count;

    foreach (float[] features in featuresList)
    {
        for (int i = 0; i < meanFeatures.Length; i++)
        {
            meanFeatures[i] = features[i];
        }
    }

    for (int i = 0; i < meanFeatures.Length; i++)
    {
        meanFeatures[i] /= totalCount;
    }

    return meanFeatures ;
}


   private static Image<Rgba32> ConvertImageRgbToHsv(Image<Rgba32> image)
   {
    var hsvImage = new Image<Rgba32>(image.Width, image.Height);
       for (var j = 0; j < image.Height; j++)
       {
           for (var i = 0; i < image.Width; i++)
           {
               var r = image[i,j].R / 255;
               var g = image[i,j].G / 255;
               var b = image[i,j].B / 255;
               var max = Math.Max(r, Math.Max(g, b));
               var min = Math.Min(r, Math.Min(g,b));
               var c = max - min;
               float h=0, s=0, v=max;
               s = (v != 0) ? c / v : 0;
               if (c != 0)
               {
                   if (max == r) h = 60* (float)(g - b) / c;
                   if (max == g) h = 120 + 60 * (float)(b - r) / c;
                   if (max == b) h = 240 + 60 *(float) (r - g) / c;
               }
               else  { h = 0; }
               if (h < 0) h += 360;
               hsvImage[i, j] = new Rgba32(h * 255 / 360, s * 255, v * 255,image[i,j].A);
           }
       }
       return hsvImage;
   }
   
   
    private float[] CalculateHistogram(Image<Rgba32> image, Image<L8> mask)
    {
        int binsH = bins.Item1;
        int binsS = bins.Item2;
        int binsV = bins.Item3;
        int[] histogram = new int[binsS * binsH * binsV];
        for (int y = 0; y < image.Height; y++)
        {
            for (int x = 0; x < image.Width; x++)
            {
                if (mask[x, y] == new L8(255))
                {
                    Rgba32 pixel = image[x, y];
                    int binH = (int)Math.Round((pixel.R / 255.0) * (binsH - 1));
                    int binS = (int)Math.Round((pixel.G / 255.0) * (binsS - 1));
                    int binV = (int)Math.Round((pixel.B / 255.0) * (binsV - 1));
                    int index = (binH * binsS * binsV) + (binS * binsV) + binV;
                    histogram[index]++;
                }
            }
        }
        float[] normalizedHist = new float[binsS * binsV * binsH];
        int totalCount = image.Width * image.Height;
        for (int i = 0; i < normalizedHist.Length; i++)
        {
            normalizedHist[i] = (float)histogram[i] / totalCount;
        }
        return normalizedHist;
    }

    private bool IsInsideEllipse(int x, int y, int cx, int cy, int ex, int ey)
    {
        double dx = x - cx;
        double dy = y - cy;
        double value = (dx * dx) / (ex * ex) + (dy * dy) / (ey * ey);
        return value <= 1;
    }
}
