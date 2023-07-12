using BackendLi.Entities;

namespace BackendLi.DTOs;

public class ModalDto
{
    public ModalDto(bool isFollowing, User following)
    {
        IsFollowing = isFollowing;
        Following = following;
    }

    public bool IsFollowing { set; get; }
    public User Following { set; get; }
}