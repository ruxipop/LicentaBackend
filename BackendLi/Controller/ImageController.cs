using System.Net;
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
    private readonly IEditorService _editorService;
    private readonly IFollowService _followService;
    private readonly IWritePhotoService _writePhotoService;
    private readonly Searcher _searcher;
    private readonly ColorDescriptor _colorDescriptor;

    public ImageController(IImageService imageService,IFollowService followService,IWritePhotoService writePhotoService,IEditorService editorService)
    {
        _imageService = imageService;
        _followService = followService;
        _writePhotoService = writePhotoService;
        _editorService = editorService;
        _searcher = new Searcher("fisier.csv");
        _colorDescriptor = new ColorDescriptor((8, 12, 3));
    }

    [AllowAnonymous]
    [HttpGet]
    public IEnumerable<  Photo > GetImages()
    {

        return _imageService.GetImages();
    }

    [AllowAnonymous]
    [HttpGet("getImage/{id}")]
    public IActionResult GetImage(int id)
    {
        Photo ? image = _imageService.GetImageById(id);
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

        var finalImages= new List<ImageResponse>();
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

            var images = _imageService.GetPaginatedImages(pageNb, pageSize, type, categories);
         
            foreach (var image in images)
            {
                var check = _editorService.IsImageVoted(image.Id);
                finalImages.Add(new ImageResponse(image,check));
            }
            return Ok(finalImages);
        }

        var images2 = _imageService.GetPaginatedImages(pageNb, pageSize, type, null);
        foreach (var image in images2)
        {
            var check = _editorService.IsImageVoted(image.Id);
            finalImages.Add(new ImageResponse(image,check));
        }
        return Ok(finalImages);
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
    [HttpGet("getImagesByAuthorId")]
    public IEnumerable<  Photo > GetImagesByType([FromQuery] int pageNb,[FromQuery] int pageSize,[FromQuery] int userId)
    {
        return _imageService.GetImagesByAuthorId(pageNb,pageSize,userId);
    }
    
    [AllowAnonymous]
    [HttpGet("getImageLikes")]
    public IActionResult GetImageLikes([FromQuery] int id,[FromQuery] int pageNb,[FromQuery] int pageSize,[FromQuery] int userId,[FromQuery] string type)
    {
        List<ModalDto> likesModalDtos = new List<ModalDto>();
        if (type == "Likes")
        {
            var likes= _imageService.GetImageLikes(pageNb, pageSize, id).ToList();

            var following = _followService.GetAllFollowing(userId).ToList();

            foreach (var like in likes)
            {
                if (following.Any(i => i.FollowingId == like.UserId))
                {
                    likesModalDtos.Add(new ModalDto(true,like.User));
                }
                else
                {
                    likesModalDtos.Add(new ModalDto(false,like.User));
                }
            }
        }else if(type=="Followers")

        {
            var followers = _followService.GetAllFollowersForUserPage(id,pageNb,pageSize).ToList();
            var following = _followService.GetAllFollowing(userId).ToList();

            foreach (var follower in followers)
            {
                if (following.Any(i => i.FollowingId == follower.FollowerId))
                {
                    likesModalDtos.Add(new ModalDto(true,follower.Follower));
                }
                else
                {
                    likesModalDtos.Add(new ModalDto(false,follower.Follower));
                }
            }  
        }else if (type ==  "Following")
        {
            var followers = _followService.GetAllFollowingPage(id,pageNb,pageSize,null).ToList();
            var following = _followService.GetAllFollowing(userId).ToList();

            foreach (var follower in followers)
            {
                if (following.Any(i => i.FollowingId == follower.FollowingId))
                {
                    likesModalDtos.Add(new ModalDto(true,follower.Following));
                }
                else
                {
                    likesModalDtos.Add(new ModalDto(false,follower.Following));
                }
            }   

          
        }

        return Ok(likesModalDtos);
    }
    
    [AllowAnonymous]
    [HttpGet("getImagesUser")]
    public IActionResult GetImagesByUserId([FromQuery] int id,[FromQuery] int pageNb,[FromQuery] int pageSize)
    {

        var images = _imageService.GetImagesByUserId(id, pageNb, pageSize);
        var finalImages = new List<ImageResponse>();
        foreach (var image in images)
        {
            var check = _editorService.IsImageVoted(image.Id);
            finalImages.Add(new ImageResponse(image,check));
        }
        return Ok(finalImages);
    }
    
    [AllowAnonymous]
    [HttpGet("getImagesLiked")]
    public IActionResult GetImagesLikedByUserId([FromQuery] int id,[FromQuery] int pageNb,[FromQuery] int pageSize)
    {
        var finalImages = new List<ImageResponse>();
        var images = _imageService.GetLikedImagesByUser(id, pageNb, pageSize);
        foreach (var image in images)
        {
            var check = _editorService.IsImageVoted(image.Id);
            finalImages.Add(new ImageResponse(image,check));
        }
        return Ok(finalImages);
    }


    [HttpPost("create")]
    [AllowAnonymous]
    public async Task<IActionResult> Create([FromBody]   Photo  image)
    {   


        if (!_imageService.CreateImage(image)) return BadRequest(new{  error ="An image with the same title already exists!"});
        _writePhotoService.AddImageToIndex(image.ImageUrl,image.Title);


      return Ok(new SuccessResponseDto("The image was uploaded successfully."));

    }
    
    [HttpDelete("{id}")]
    [AllowAnonymous]
    public IActionResult DeleteImage(int id)
    {
        Console.WriteLine("delete image");
        _imageService.DeleteImage(id);
        return Ok(new SuccessResponseDto("Your image has been deleted"));

    }

    [AllowAnonymous]
    [HttpGet("getSimi")]
    public IActionResult iep([FromQuery] string queryImagePath,string imageTitle)
    {
        var list = new List<Photo>();
        using (WebClient webClient = new WebClient())
        {
            byte[] imageData = webClient.DownloadData(queryImagePath);
            using (var imageStream = new MemoryStream(imageData))
            {
                Image<Rgba32> image = Image.Load<Rgba32>(imageStream);

                float[] queryFeatures = _colorDescriptor.DescribeImage(image);
                List<(double, string)> results = _searcher.ReadFile(queryFeatures);
                
                foreach (var result in results)
                {
                    Console.WriteLine($"Image ID: {result.Item2}, Distance: {result.Item1}");

                    if (!result.Item2.Contains(imageTitle))
                    {
                       
Console.WriteLine("ajunf");                        list.Add(_imageService.GetImageByTitle(result.Item2)!);
                    }
                }
            }
        }
        Console.WriteLine(list[0]);
        return Ok(list);
    }
}