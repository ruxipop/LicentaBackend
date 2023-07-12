using System.Net;
using BackendLi.Entities.Attributes;

namespace BackendLi.Services.ServiceImpl;

[Service(typeof(IWritePhotoService))]
public class WritePhotoService : IWritePhotoService
{
    public void AddImageToIndex(string imagePath, string imageName)
    {
        using (var webClient = new WebClient())
        {
            var imageData = webClient.DownloadData(imagePath);
            using (var imageStream = new MemoryStream(imageData))
            {
                var image = Image.Load<Rgba32>(imageStream);
              
                using (var output = new StreamWriter("fisier.txt", true))
                {
                    var cd = new ColorDescriptor((8, 12, 3));
                    var features = cd.DescribeImage(image);
                    var featureString = string.Join(" ", features);
                    output.WriteLine($"{imageName} {featureString}");
                }
            }
        }
    }
}