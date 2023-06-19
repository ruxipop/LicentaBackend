using System.Text.RegularExpressions;
using BackendLi.Entities.Attributes;

namespace BackendLi.Services;

[Service(typeof(IWritePhotoService))]

public class WritePhotoService:IWritePhotoService
{


    public  string createPhoto( string imageUrl)
    {
        
        byte[] decBytes = Convert.FromBase64String(Regex.Split(imageUrl, ",")[1]);

   
        string filename = $"{Guid.NewGuid()}.jpg";

        var assetsFolder = Path.GetFullPath("C:\\Users\\POU2CLJ\\Desktop\\lic\\LicentaFrontend\\src\\assets\\imgs");
        var imagePath = Path.Combine(assetsFolder,filename);
        using (FileStream fs = new FileStream(imagePath, FileMode.Create))
        {
            fs.Write(decBytes, 0, decBytes.Length);

        }
        
        var relativePath = "assets/imgs/" + filename;
        return
        relativePath;
    }
}