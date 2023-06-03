using BackendLi.Controller.Exceptions;
using BackendLi.DataAccess;
using BackendLi.DTOs;
using BackendLi.Entities;
using BackendLi.Entities.Attributes;
using Microsoft.EntityFrameworkCore;

namespace BackendLi.Services;


[Service(typeof(ILikeService))]

public class LikeService:ILikeService
{
    
    private readonly IRepository _repository;

    public LikeService(IRepository repository)
    {
        _repository = repository;
    }

    public SuccessResponseDto AddLikeToImage(int imageId, User user)
    {
        Image image =_repository.GetEntities<Image>().FirstOrDefault(i=>i.Id==imageId)?? throw new  ResourceNotFoundException($"Image with id {imageId} not found");
        Like? like = _repository.GetEntities<Like>().FirstOrDefault(i => i.ImageId == imageId && i.UserId == user.Id);
        if (like != null)
        {
            return new SuccessResponseDto("You have already liked this image!");
            
        }

        Like newLike = new Like(user.Id, imageId);
        using (IUnitOfWork unitOfWork = _repository.CreateUnitOfWork())
        {
          
            unitOfWork.Add(newLike);
            unitOfWork.SaveChanges();
        }

        return new SuccessResponseDto("Image liked successfully!");
    }

    public SuccessResponseDto DeleteLike(int imageId, int userId)
    {

        using (IUnitOfWork unitOfWork = _repository.CreateUnitOfWork())
        {
            Like? like = unitOfWork.GetEntities<Like>().FirstOrDefault(i => i.ImageId == imageId && i.UserId == userId);
            
            if (like!= null)
            {
                unitOfWork.Delete(like);
                unitOfWork.SaveChanges();
                return new SuccessResponseDto("Like removed successfully.");
            }

        }
        return new SuccessResponseDto("No like found for that image");

    }

 
}