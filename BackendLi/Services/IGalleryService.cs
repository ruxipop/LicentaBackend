using BackendLi.Entities;

namespace BackendLi.Services;

public interface IGalleryService
{
    public Gallery? getGalleryById(int id);

    public void UpdateGallery(Gallery gallery);
    public void CreateGallery(Gallery gallery);

}