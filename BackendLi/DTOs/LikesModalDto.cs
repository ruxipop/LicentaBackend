using BackendLi.Entities;

namespace BackendLi.DTOs;

public class LikesModalDto
{
    public LikesModalDto(bool following,Like likedImage)
    {
        Following = following;
        LikedImage = likedImage;
    }

    public bool Following { set; get; }
    public Like LikedImage { set; get; }
    
    
}