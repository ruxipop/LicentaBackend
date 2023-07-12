using BackendLi.Entities;

namespace BackendLi.DTOs;

public class ImageResponse
{
    public ImageResponse(Photo image, bool voted)
    {
        Image = image;
        Voted = voted;
    }

    public Photo Image { set; get; }
    public bool Voted { get; set; }
}