using BackendLi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackendLi.Controller;

[Route("api/[controller]")]
[ApiController]
public class LikeController : ControllerBase
{
    private readonly ILikeService _likeService;
    private readonly IUserService _userService;

    public LikeController(IUserService userService, ILikeService likeService)
    {
        _userService = userService;
        _likeService = likeService;
    }

    [Authorize]
    [HttpPost]
    public IActionResult AddLikeToImage([FromQuery] int imageId)
    {
        var user = _userService.GetCurrentUser(Request);
        if (user != null)
        {
            var successResponseDto = _likeService.AddLikeToImage(imageId, user);
            return Ok(successResponseDto);
        }

        return BadRequest();
    }

    [Authorize]
    [HttpDelete]
    public IActionResult DeleteFollow([FromQuery] int imageId)
    {
        var user = _userService.GetCurrentUser(Request);
        if (user != null)
        {
            var successResponseDto = _likeService.DeleteLike(imageId, user.Id);
            return Ok(successResponseDto);
        }

        return BadRequest();
    }
}