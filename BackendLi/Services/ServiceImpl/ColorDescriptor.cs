namespace BackendLi.Services.ServiceImpl;

public class ColorDescriptor
{
    private readonly (int, int, int) _bins;

    public ColorDescriptor((int, int, int) bins)
    {
        _bins = bins;
    }

    public float[] DescribeImage(Image<Rgba32> image)
    {
        var binsH = _bins.Item1;
        var binsS = _bins.Item2;
        var binsV = _bins.Item3;

        var featuresList = new List<float[]>();
        image = ConvertImageRgbToHsv(image);

        var width = image.Width;
        var height = image.Height;

        var cx = width / 2;
        var cy = height / 2;

        int[,] segments = { { 0, cx, 0, cy }, { cx, width, 0, cy }, { cx, width, cy, height }, { 0, cx, cy, height } };

        var ex = width * 3 / 8;
        var ey = height * 3 / 8;

        using (var ellipMask = new Image<L8>(width, height))
        {
            for (var y = 0; y < height; y++)
            for (var x = 0; x < width; x++)
                if (IsInsideEllipse(x, y, cx, cy, ex, ey))
                    ellipMask[x, y] = new L8(255);
                else
                    ellipMask[x, y] = new L8(0);

            for (var i = 0; i < segments.GetLength(0); i++)
            {
                var startX = segments[i, 0];
                var endX = segments[i, 1];
                var startY = segments[i, 2];
                var endY = segments[i, 3];

                using (var cornerMask = new Image<L8>(width, height))
                {
                    for (var y = startY; y < endY; y++)
                    for (var x = startX; x < endX; x++)
                        if (ellipMask[x, y] == new L8(255))
                            cornerMask[x, y] = new L8(0);
                        else
                            cornerMask[x, y] = new L8(255);

                    var hist = CalculateHistogram(image, cornerMask);
                    featuresList.Add(hist);
                }
            }

            var ellipseHist = CalculateHistogram(image, ellipMask);
            featuresList.Add(ellipseHist);
        }

        var meanFeatures = new float[binsH * binsV * binsS];
        var totalCount = featuresList.Count;

        foreach (var features in featuresList)
            for (var i = 0; i < meanFeatures.Length; i++)
                meanFeatures[i] = features[i];

        for (var i = 0; i < meanFeatures.Length; i++) meanFeatures[i] /= totalCount;
        return meanFeatures;
    }


    private static Image<Rgba32> ConvertImageRgbToHsv(Image<Rgba32> image)
    {
        var hsvImage = new Image<Rgba32>(image.Width, image.Height);
        for (var j = 0; j < image.Height; j++)
        for (var i = 0; i < image.Width; i++)
        {
            var r = image[i, j].R / 255;
            var g = image[i, j].G / 255;
            var b = image[i, j].B / 255;
            var max = Math.Max(r, Math.Max(g, b));
            var min = Math.Min(r, Math.Min(g, b));
            var c = max - min;
            float h = 0, s = 0, v = max;
            s = v != 0 ? c / v : 0;
            if (c != 0)
            {
                if (max == r) h = 60 * (float)(g - b) / c;
                if (max == g) h = 120 + 60 * (float)(b - r) / c;
                if (max == b) h = 240 + 60 * (float)(r - g) / c;
            }
            else
            {
                h = 0;
            }

            if (h < 0) h += 360;
            hsvImage[i, j] = new Rgba32(h * 255 / 360, s * 255, v * 255, image[i, j].A);
        }

        return hsvImage;
    }


    private float[] CalculateHistogram(Image<Rgba32> image, Image<L8> mask)
    {
        var binsH = _bins.Item1;
        var binsS = _bins.Item2;
        var binsV = _bins.Item3;
        var histogram = new int[binsS * binsH * binsV];
        for (var y = 0; y < image.Height; y++)
        for (var x = 0; x < image.Width; x++)
            if (mask[x, y] == new L8(255))
            {
                var pixel = image[x, y];
                var binH = (int)Math.Round(pixel.R / 255.0 * (binsH - 1));
                var binS = (int)Math.Round(pixel.G / 255.0 * (binsS - 1));
                var binV = (int)Math.Round(pixel.B / 255.0 * (binsV - 1));
                var index = binH * binsS * binsV + binS * binsV + binV;
                histogram[index]++;
            }

        var normalizedHist = new float[binsS * binsV * binsH];
        var totalCount = image.Width * image.Height;
        for (var i = 0; i < normalizedHist.Length; i++) normalizedHist[i] = (float)histogram[i] / totalCount;
        return normalizedHist;
    }

    private bool IsInsideEllipse(int x, int y, int cx, int cy, int ex, int ey)
    {
        double dx = x - cx;
        double dy = y - cy;
        var value = dx * dx / (ex * ex) + dy * dy / (ey * ey);
        return value <= 1;
    }
}