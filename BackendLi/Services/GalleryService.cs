using BackendLi.DataAccess;
using BackendLi.Entities;
using BackendLi.Entities.Attributes;
using Microsoft.EntityFrameworkCore;

namespace BackendLi.Services;


[Service(typeof(IGalleryService))]
public class GalleryService :IGalleryService
{
    
    private readonly IRepository _repository;


    public GalleryService(IRepository repository)
    {
        _repository = repository;
    }
    public Gallery? getGalleryById(int id)
    {
        return _repository.GetEntities<Gallery>()
            .Include(x => x.Images!)
            .ThenInclude(i => i.Autor).Include(u=>u.User!)
            .FirstOrDefault(x => x.Id == id);

    }

    public void UpdateGallery(Gallery gallery)
    {
      
            using (IUnitOfWork unitOfWork = _repository.CreateUnitOfWork())
            {
                unitOfWork.Update(gallery);
                unitOfWork.SaveChanges();
                
            }



    }
    
    public void CreateGallery(Gallery gallery)
    {
      
        using (IUnitOfWork unitOfWork = _repository.CreateUnitOfWork())
        {
            unitOfWork.Add(gallery);
            unitOfWork.SaveChanges();
                
        }
        
    } 

}