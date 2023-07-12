using BackendLi.DTOs;
using BackendLi.Entities;

namespace BackendLi.Services;

public interface ILikeService
{
    public SuccessResponseDto AddLikeToImage(int imageId, User user);
    public SuccessResponseDto DeleteLike(int imageId, int userId);
}