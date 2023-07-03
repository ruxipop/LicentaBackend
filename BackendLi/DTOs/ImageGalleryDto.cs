using BackendLi.Entities;

namespace BackendLi.DTOs;

public class ImageGalleryDto
{
    public Photo Image { get; set; }
    public Gallery Gallery { get; set; }
}