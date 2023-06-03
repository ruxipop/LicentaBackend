using BackendLi.Controller.Exceptions;
using BackendLi.DataAccess;
using BackendLi.DTOs;
using BackendLi.Entities;
using BackendLi.Entities.Attributes;
using Microsoft.EntityFrameworkCore;

namespace BackendLi.Services;

[Service(typeof(IFollowService))]
public class FollowService:IFollowService
{
    private readonly IRepository _repository;


    public FollowService(IRepository repository)
    {
        _repository = repository;
    }

    
    //cine il urmare pe el
    public IEnumerable<Follow> GetAllFollowersForUser(int userId)
    {
        return _repository.GetEntities<Follow>().Include(i => i.Follower)
            .Where(i => i.FollowingId == userId).ToList();
        
    }

    //pe cine urmareste el
    public IEnumerable<Follow> GetAllFollowing(int userId)
    {
        return _repository.GetEntities<Follow>().Where(i => i.FollowerId == userId)
           
            .ToList(); ;
    }
    
    
    public SuccessResponseDto DeleteFollow(int followerId, int followingId)
    {

        using (IUnitOfWork unitOfWork = _repository.CreateUnitOfWork())
        {
            Follow? follow = _repository.GetEntities<Follow>()
                .FirstOrDefault(i => i.FollowerId == followerId && i.FollowingId == followingId);            
            if (follow!= null)
            {
                unitOfWork.Delete(follow);
                unitOfWork.SaveChanges();
                return new SuccessResponseDto("Follow removed successfully.");
            }

        }
        return new SuccessResponseDto("No follow");

    }
    
    
    public SuccessResponseDto AddFollow(User user, int followingId)
    {
        User following=_repository.GetEntities<User>().FirstOrDefault(i=>i.Id==followingId) ?? throw new  ResourceNotFoundException($"User with id {followingId} not found");
        Follow? follow = _repository.GetEntities<Follow>()
            .FirstOrDefault(i => i.FollowerId == user.Id && i.FollowingId == followingId);   
if (follow != null)
        {
            return new SuccessResponseDto("You have already follow this user!");
            
        }

Follow newFollow = new Follow(user.Id, followingId);
using (IUnitOfWork unitOfWork = _repository.CreateUnitOfWork())
        {
          
            unitOfWork.Add(newFollow);
            unitOfWork.SaveChanges();
        }

        return new SuccessResponseDto("Follow successfully!");
    }


    public bool IsUserFollowing(int followerId, int followingId)
    {
        Follow? follow = _repository.GetEntities<Follow>()
            .FirstOrDefault(i => i.FollowingId == followingId && i.FollowerId == followerId);
        return follow != null;
    }
}