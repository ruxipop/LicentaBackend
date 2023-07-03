using System.Net;
using System.Text.RegularExpressions;
using BackendLi.Entities.Attributes;

namespace BackendLi.Services;

[Service(typeof(IWritePhotoService))]

public class WritePhotoService:IWritePhotoService
{
  
    public void AddImageToIndex(string imagePath,string imageName)
    {
        using (WebClient webClient = new WebClient())
        {
            byte[] imageData = webClient.DownloadData(imagePath);
            using (var imageStream = new MemoryStream(imageData))
            {
                Image<Rgba32> image = Image.Load<Rgba32>(imageStream);

                using (var output = new StreamWriter("fisier.csv", append: true))
                {

                    ColorDescriptor cd = new ColorDescriptor((8, 12, 3));
                    float[] features = cd.DescribeImage(image);
                    string featureString = string.Join(",", features);
                    output.WriteLine($"{imageName},{featureString}");
                }
            }
        }
    }


}