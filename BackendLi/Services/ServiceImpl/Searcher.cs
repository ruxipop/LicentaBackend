using System.Globalization;

namespace BackendLi.Services.ServiceImpl;

public class Searcher
{
    private readonly string _indexPath;

    public Searcher(string indexPath)
    {
        _indexPath = indexPath;
    }


    public List<(double, string)> ReadFile(string name, int limit = 5)
    {
        var results = new List<(double, string)>();

        float[] queryFeatures = null;
        var lines = File.ReadAllLines(_indexPath);
        var searchedLine = lines.FirstOrDefault(line => line.StartsWith(name));

        if (searchedLine != null)
        {
            var row = searchedLine.Split(' ');
            queryFeatures = row.Skip(1).Select(s => float.Parse(s, CultureInfo.InvariantCulture)).ToArray();
        }

        foreach (var line in lines)
        {
            float[] features;

            var row = line.Split(' ');

            var imageId = row[0];
            features = row.Skip(1).Select(s => float.Parse(s, CultureInfo.InvariantCulture)).ToArray();
            var distance = ChiSquaredDistance(queryFeatures, features);
            results.Add((distance, imageId));
        }


        results = results.OrderBy(r => r.Item1).ToList();
        return results.Take(limit).ToList();
    }


    private static double ChiSquaredDistance(float[] a, float[] b, double epsilon = 1e-10)
    {
        double distance = 0;
        for (var i = 0; i < a.Length; i++)
            distance += Math.Pow(a[i] - b[i], 2) / (a[i] + a[i] + epsilon);
        distance *= 0.5;
        return distance;
    }
}