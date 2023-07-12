using BackendLi.DataAccess;
using BackendLi.DTOs;
using BackendLi.Entities;
using BackendLi.Entities.Attributes;
using Microsoft.EntityFrameworkCore;

namespace BackendLi.Services.ServiceImpl;

[Service(typeof(IGalleryService))]
public class GalleryService : IGalleryService
{
    private readonly IRepository _repository;


    public GalleryService(IRepository repository)
    {
        _repository = repository;
    }

    public Gallery? GetGalleryById(int id)
    {
        return _repository.GetEntities<Gallery>()
            .Include(x => x.Images!)
            .ThenInclude(i => i.Autor).Include(u => u.User)
            .Include(x => x.Images!).ThenInclude(i => i.Likes)
            .FirstOrDefault(x => x.Id == id);
    }

    public void UpdateGallery(Gallery gallery)
    {
        using (var unitOfWork = _repository.CreateUnitOfWork())
        {
            unitOfWork.Update(gallery);
            unitOfWork.SaveChanges();
        }
    }

    public void CreateGallery(Gallery gallery)
    {
        using (var unitOfWork = _repository.CreateUnitOfWork())
        {
            unitOfWork.Add(gallery);
            unitOfWork.SaveChanges();
        }
    }


    public IEnumerable<Gallery> GetAllGalleries(int userId, int pageNb, int pageSize, string? searchTerm)


    {
        searchTerm = string.IsNullOrEmpty(searchTerm) ? null : searchTerm.ToLower();

        return _repository.GetEntities<Gallery>()
            .Where(g => g.UserId == userId)
            .Include(x => x.Images!)
            .ThenInclude(i => i.Autor).Include(u => u.User!)
            .Where(i => searchTerm == null || i.Name.Contains(searchTerm))
            .Skip((pageNb - 1) * pageSize)
            .Take(pageSize)
            .ToList();
    }

    public SuccessResponseDto AddGalleryToImage(Photo image, Gallery gallery)
    {
        using (var unitOfWork = _repository.CreateUnitOfWork())
        {
            image.GalleryId = gallery.Id;
            unitOfWork.Update(image);
            unitOfWork.SaveChanges();
        }

        return new SuccessResponseDto("Photo successfully added!");
    }

    public SuccessResponseDto RemoveImageFromGallery(Photo image, Gallery gallery)
    {
        using (var unitOfWork = _repository.CreateUnitOfWork())
        {
            image.GalleryId = null;
            unitOfWork.Update(image);
            unitOfWork.SaveChanges();
        }

        return new SuccessResponseDto("Photo was removed from Gallery!");
    }

    public void DeleteGallery(int id)
    {
        using (var unitOfWork = _repository.CreateUnitOfWork())
        {
            var gallery = _repository.GetEntities<Gallery>().FirstOrDefault(g => g.Id == id);
            if (gallery != null)
            {
                var images = _repository.GetEntities<Photo>().Where(i => i.GalleryId == id);
                foreach (var image in images)
                {
                    image.GalleryId = null;
                    unitOfWork.Update(image);
                    unitOfWork.SaveChanges();
                }
            }

            unitOfWork.Delete(gallery!);
            unitOfWork.SaveChanges();
        }
    }
}