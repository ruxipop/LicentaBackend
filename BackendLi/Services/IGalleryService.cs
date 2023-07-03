using BackendLi.DTOs;
using BackendLi.Entities;

namespace BackendLi.Services;

public interface IGalleryService
{
    public Gallery? GetGalleryById(int id);

    public IEnumerable<Gallery> GetAllGalleries(int userId, int pageNb, int pageSize, string? searchTerm);
    public void UpdateGallery(Gallery gallery);
    public void CreateGallery(Gallery gallery);
    public SuccessResponseDto AddGalleryToImage(Photo image, Gallery gallery);
    public SuccessResponseDto RemoveImageFromGallery(Photo image, Gallery gallery);
    public void DeleteGallery(int id);
}