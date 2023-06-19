using BackendLi.DataAccess.Enums;
using BackendLi.Entities;

namespace BackendLi.Services;

public interface IImageService
{
   IEnumerable<Image> GetImages();
   IEnumerable<Image> GetPaginatedImages(int pageNb, int pageSize,string type,List<string>?category);
   Image? GetImageById(int id);
   IEnumerable<Image> GetImagesByTypeAndCategory(string type,List<string>?  category);
   string GetImageType(int id);
   IEnumerable<Image> GetImagesByAuthorId(int pageNb,int pageSize,int authorId);

   IEnumerable<Like> GetImageLikes(int pageNb, int pageSize, int imageId);
   IEnumerable<Image> GetImagesByUserId(int userId, int pageNb, int pageSize);
   IEnumerable<Image> GetLikedImagesByUser(int userId, int pageNb, int pageSize);

   public void CreateImage(Image image);
   public void DeleteImage(int id);


}