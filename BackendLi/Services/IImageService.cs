using BackendLi.Entities;

namespace BackendLi.Services;

public interface IImageService
{
    IEnumerable<Photo> GetImages();
    IEnumerable<Photo> GetPaginatedImages(int pageNb, int pageSize, string type, List<string>? category);
    Photo? GetImageById(int id);
    IEnumerable<Photo> GetImagesByTypeAndCategory(string type, List<string>? category);
    string GetImageType(int id);
    IEnumerable<Photo> GetImagesByAuthorId(int pageNb, int pageSize, int authorId);

    IEnumerable<Like> GetImageLikes(int pageNb, int pageSize, int imageId);
    IEnumerable<Photo> GetImagesByUserId(int userId, int pageNb, int pageSize);
    IEnumerable<Photo> GetLikedImagesByUser(int userId, int pageNb, int pageSize);

    public bool CreateImage(Photo image);
    public void DeleteImage(int id);

    public Photo? GetImageByTitle(string url);
}