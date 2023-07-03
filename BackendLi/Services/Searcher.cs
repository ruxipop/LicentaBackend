namespace BackendLi.Services;

public class Searcher
{
    private readonly string indexPath;

    public Searcher(string indexPath)
    {
        this.indexPath = indexPath;
            
    }

        
        
    public List<(double, string)> ReadFile(float[] queryFeatures, int limit = 5)
    {
        var results = new List<(double, string)>();
        using (var reader = new StreamReader(indexPath))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                var row = line.Split(',');
                var imageId = row[0];
                var  features = row.Skip(1).Select(float.Parse).ToArray();
                var distance = ChiSquaredDistance(features, queryFeatures);
                results.Add((distance, imageId));
            }
        }
        
        results = results.OrderBy(r => r.Item1).ToList();
        return results.Take(limit).ToList();
    }


    private static double ChiSquaredDistance(float[] a, float[] b, double epsilon = 1e-10)
    {
        double distance = 0;
        for (int i = 0; i < a.Length; i++)
        {
            distance +=  Math.Pow(a[i] - b[i], 2)/( a[i] + a[i] + epsilon);
        }
        distance *= 0.5;
        return distance;
    }
}
