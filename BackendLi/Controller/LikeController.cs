using BackendLi.DTOs;
using BackendLi.Entities;
using BackendLi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackendLi.Controller;


[Route("api/[controller]")]
[ApiController]
public class LikeController:ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILikeService _likeService;

    public LikeController(IUserService userService, ILikeService likeService)
    {
        _userService = userService;
        _likeService = likeService;
    }

    [Authorize]
    [HttpPost]
    public IActionResult AddLikeToImage([FromQuery] int imageId)
    {
      
        User? user = _userService.GetCurrentUser(Request);
        if (user != null)
        {
            SuccessResponseDto successResponseDto = _likeService.AddLikeToImage(imageId, user);
            return Ok(successResponseDto);
        }

        return BadRequest();

    }
    [Authorize]
    [HttpDelete]
    public IActionResult DeleteFollow([FromQuery] int imageId)
    {
      
        
        User? user = _userService.GetCurrentUser(Request);
        if (user != null)
        {
            SuccessResponseDto successResponseDto = _likeService.DeleteLike(imageId, user.Id);
            return Ok(successResponseDto);
        }

        return BadRequest();

    }
    
    
}