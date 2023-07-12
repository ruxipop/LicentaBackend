using BackendLi.DataAccess;
using BackendLi.DTOs;
using BackendLi.Entities;
using BackendLi.Entities.Attributes;

namespace BackendLi.Services.ServiceImpl;

[Service(typeof(ILikeService))]
public class LikeService : ILikeService
{
    private readonly IRepository _repository;

    public LikeService(IRepository repository)
    {
        _repository = repository;
    }

    public SuccessResponseDto AddLikeToImage(int imageId, User user)
    {
        var like = _repository.GetEntities<Like>().FirstOrDefault(i => i.ImageId == imageId && i.UserId == user.Id);
        if (like != null) return new SuccessResponseDto("You have already liked this image!");

        var newLike = new Like(user.Id, imageId);
        using (var unitOfWork = _repository.CreateUnitOfWork())
        {
            unitOfWork.Add(newLike);
            unitOfWork.SaveChanges();
        }

        return new SuccessResponseDto("Image liked successfully!");
    }

    public SuccessResponseDto DeleteLike(int imageId, int userId)
    {
        using (var unitOfWork = _repository.CreateUnitOfWork())
        {
            var like = unitOfWork.GetEntities<Like>().FirstOrDefault(i => i.ImageId == imageId && i.UserId == userId);

            if (like != null)
            {
                unitOfWork.Delete(like);
                unitOfWork.SaveChanges();
                return new SuccessResponseDto("Like removed successfully.");
            }
        }

        return new SuccessResponseDto("No like found for that image");
    }
}