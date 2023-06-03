using BackendLi.DTOs;
using BackendLi.Entities;

namespace BackendLi.Services;

public interface IFollowService
{
    public IEnumerable<Follow> GetAllFollowersForUser(int userId);
    public IEnumerable<Follow> GetAllFollowing(int userId);
    public SuccessResponseDto DeleteFollow(int followerId, int followingId);
    public SuccessResponseDto AddFollow(User user, int followingId);
    public bool IsUserFollowing(int followerId, int followingId);

}