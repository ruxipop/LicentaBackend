using System.Net.Mime;
using BackendLi.DataAccess;
using BackendLi.DataAccess.Enums;
using BackendLi.DTOs;
using BackendLi.Entities;
using BackendLi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackendLi.Controller;

[Route("api/[controller]")]
[ApiController]
public class ImageController : ControllerBase
{

    private readonly IImageService _imageService;
    private readonly IFollowService _followService;
    public ImageController(IImageService imageService,IFollowService followService)
    {
        _imageService = imageService;
        _followService = followService;
    }

    [AllowAnonymous]
    [HttpGet]
    public IEnumerable<Image> GetImages()
    {

        return _imageService.GetImages();
    }

    [AllowAnonymous]
    [HttpGet("getImage/{id}")]
    public IActionResult GetImage(int id)
    {
        Image? image = _imageService.GetImageById(id);
        if (image != null)
        {
            return Ok(image);
        }

        return NotFound("Image not found");

    }

    [AllowAnonymous]
    [HttpGet("pages")]
    public IActionResult GetImages([FromQuery] int pageNb, [FromQuery] int pageSize, [FromQuery] string type, [FromQuery] string? category)
    {
        Console.WriteLine(category);

        List<string> categories = new List<string>();
        if (!string.IsNullOrEmpty(category))
        {
            if (category.Contains(','))
            {
                categories = category.Split(',').ToList();
            }
            else
            {
                categories.Add(category);
            }

            var image2 = _imageService.GetPaginatedImages(pageNb, pageSize, type, categories);
            return Ok(image2);
        }

        var image = _imageService.GetPaginatedImages(pageNb, pageSize, type, null);
        Console.WriteLine("acum " + image);
        return Ok(image);
    }


    // [AllowAnonymous]
    // [HttpGet("getImagesByType")]
    // public IEnumerable<Image> GetImagesByTypeAndCategory([FromQuery] string typeImage,[FromQuery] string? category)
    // {
    //     return _imageService.GetImagesByTypeAndCategory(typeImage,category);
    // }

    [AllowAnonymous]
    [HttpGet("getImageType/{id}")]
    public IActionResult IsImagePopular(int id)
    {
        string imageType = _imageService.GetImageType(id);

        return Ok(new { imageType = imageType });
    }

    [AllowAnonymous]
    [HttpGet("getImagesByAuthorId/{id}")]
    public IEnumerable<Image> GetImagesByType(int id)
    {
        Console.WriteLine("intruuuuu ");
        return _imageService.GetImagesByAuthorId(id);
    }
    
    [AllowAnonymous]
    [HttpGet("getImageLikes")]
    public IActionResult GetImageLikes([FromQuery] int id,[FromQuery] int pageNb,[FromQuery] int pageSize,[FromQuery] int userId)
    {
        List<LikesModalDto> likesModalDtos = new List<LikesModalDto>();
        var likes= _imageService.GetImageLikes(pageNb, pageSize, id).ToList();

        var following = _followService.GetAllFollowing(userId).ToList();

        foreach (var like in likes)
        {
            if (following.Any(i => i.FollowingId == like.UserId))
            {
                likesModalDtos.Add(new LikesModalDto(true,like));
            }
            else
            {
                likesModalDtos.Add(new LikesModalDto(false,like));
            }
        }
        return Ok(likesModalDtos);
    }
    
    [AllowAnonymous]
    [HttpGet("getImagesUser")]
    public IActionResult GetImagesByUserId([FromQuery] int id,[FromQuery] int pageNb,[FromQuery] int pageSize)
    {

        var likes = _imageService.GetImagesByUserId(id, pageNb, pageSize);
        return Ok(likes);
    }
    
    [AllowAnonymous]
    [HttpGet("getImagesLiked")]
    public IActionResult GetImagesLikedByUserId([FromQuery] int id,[FromQuery] int pageNb,[FromQuery] int pageSize)
    {

        var likes = _imageService.GetLikedImagesByUser(id, pageNb, pageSize);
        return Ok(likes);
    }
}