using BackendLi.DTOs;
using BackendLi.Entities;
using BackendLi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackendLi.Controller;

[Route("api/[controller]")]
[ApiController]
public class GalleryController : ControllerBase
{
    private readonly IGalleryService _galleryService;

    public GalleryController(IGalleryService galleryService)
    {
        _galleryService = galleryService;
    }

    [HttpGet("{id}")]
    [Authorize]
    public IActionResult GetGalleryById(int id)
    {
        var gallery = _galleryService.GetGalleryById(id);
        if (gallery != null) return Ok(gallery);

        return NotFound();
    }

    [HttpGet("allGalleries")]
    [Authorize]
    public IActionResult GetAllGalleries([FromQuery] int userId, [FromQuery] int pageNb, [FromQuery] int pageSize,
        [FromQuery] string? searchTerm)
    {
        var galleries = _galleryService.GetAllGalleries(userId, pageNb, pageSize, searchTerm);

        return Ok(galleries);
    }

    [HttpPut("update")]
    [Authorize]
    public IActionResult Update([FromBody] Gallery gallery)
    {
        _galleryService.UpdateGallery(gallery);

        return Ok();
    }

    [HttpPost("create")]
    [Authorize]
    public IActionResult Create([FromBody] Gallery gallery)
    {
        _galleryService.CreateGallery(gallery);
        return Ok(gallery);
    }

    [HttpPost("addPhoto")]
    [Authorize]
    public IActionResult AddPhoto([FromBody] ImageGalleryDto imageGalleryDto)
    {
        return Ok(_galleryService.AddGalleryToImage(imageGalleryDto.Image, imageGalleryDto.Gallery));
    }

    [HttpPost("removePhoto")]
    [Authorize]
    public IActionResult RemovePhoto([FromBody] ImageGalleryDto imageGalleryDto)
    {
        return Ok(_galleryService.RemoveImageFromGallery(imageGalleryDto.Image, imageGalleryDto.Gallery));
    }

    [HttpDelete("{id}")]
    [Authorize]
    public IActionResult DeleteGallery(int id)
    {
        _galleryService.DeleteGallery(id);
        return Ok(new SuccessResponseDto("Your gallery has been deleted"));
    }
}