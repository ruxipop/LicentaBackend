using BackendLi.DataAccess;
using BackendLi.Entities;
using BackendLi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackendLi.Controller;

[Route("api/[controller]")]
[ApiController]
public class GalleryController:ControllerBase
{
    private readonly IGalleryService _galleryService;

    public GalleryController(IGalleryService galleryService)
    {
        _galleryService = galleryService;
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public IActionResult getGalleryById(int id)
    {
        var gallery = _galleryService.getGalleryById(id);
        if (gallery != null)
        {
            return Ok(gallery);
        }

        return NotFound();
    }
    
    
    [HttpPut("update")]
    [AllowAnonymous]
    public IActionResult Update([FromBody] Gallery gallery)
    {
       _galleryService.UpdateGallery(gallery);

        return Ok();
    }
    
    [HttpPost("create")]
    [AllowAnonymous]
    public IActionResult Create([FromBody] Gallery gallery)
    {
        Console.WriteLine("sal");
        _galleryService.CreateGallery(gallery);

    
            // Returnați ID-ul componentei într-un obiect de răspuns
            var response = new { Id = gallery.Id };
        return Ok(response);

    }
}