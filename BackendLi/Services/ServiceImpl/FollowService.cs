using BackendLi.DataAccess;
using BackendLi.DTOs;
using BackendLi.Entities;
using BackendLi.Entities.Attributes;
using Microsoft.EntityFrameworkCore;

namespace BackendLi.Services.ServiceImpl;

[Service(typeof(IFollowService))]
public class FollowService : IFollowService
{
    private readonly IRepository _repository;


    public FollowService(IRepository repository)
    {
        _repository = repository;
    }


    public IEnumerable<Follow> GetAllFollowersForUser(int userId)
    {
        return _repository.GetEntities<Follow>().Include(i => i.Follower)
            .Where(i => i.FollowingId == userId).ToList();
    }

    public IEnumerable<Follow> GetAllFollowersForUserPage(int userId, int pageNb, int pageSize)
    {
        return _repository.GetEntities<Follow>().Include(i => i.Follower)
            .Where(i => i.FollowingId == userId).Skip((pageNb - 1) * pageSize).Take(pageSize)
            .ToList();
    }

    public IEnumerable<Follow> GetAllFollowingPage(int userId, int pageNb, int pageSize, string? searchTerm)
    {
        searchTerm = string.IsNullOrEmpty(searchTerm) ? null : searchTerm.ToLower();

        return _repository.GetEntities<Follow>()
            .Include(i => i.Following)
            .Include(i => i.Follower)
            .Where(i => i.FollowerId == userId &&
                        (searchTerm == null ||
                         i.Following.Name.ToLower().Contains(searchTerm) ||
                         i.Following.Username.ToLower().Contains(searchTerm)))
            .Skip((pageNb - 1) * pageSize)
            .Take(pageSize)
            .ToList();
    }


    public IEnumerable<Follow> GetAllFollowing(int userId)
    {
        return _repository.GetEntities<Follow>().Include(i => i.Following).Include(i => i.Follower)
            .Where(i => i.FollowerId == userId)
            .ToList();
    }


    public SuccessResponseDto DeleteFollow(int followerId, int followingId)
    {
        using (var unitOfWork = _repository.CreateUnitOfWork())
        {
            var follow = _repository.GetEntities<Follow>()
                .FirstOrDefault(i => i.FollowerId == followerId && i.FollowingId == followingId);
            if (follow != null)
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
        var follow = _repository.GetEntities<Follow>()
            .FirstOrDefault(i => i.FollowerId == user.Id && i.FollowingId == followingId);
        if (follow != null) return new SuccessResponseDto("You have already follow this user!");

        var newFollow = new Follow(user.Id, followingId);
        using (var unitOfWork = _repository.CreateUnitOfWork())
        {
            unitOfWork.Add(newFollow);
            unitOfWork.SaveChanges();
        }

        return new SuccessResponseDto("Follow successfully!");
    }


    public bool IsUserFollowing(int followerId, int followingId)
    {
        var follow = _repository.GetEntities<Follow>()
            .FirstOrDefault(i => i.FollowingId == followingId && i.FollowerId == followerId);
        return follow != null;
    }
}