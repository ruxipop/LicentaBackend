using BackendLi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackendLi.Controller;

[Route("api/[controller]")]
[ApiController]
public class FollowController : ControllerBase
{
    private readonly IFollowService _followService;
    private readonly IUserService _userService;

    public FollowController(IFollowService followService, IUserService userService)
    {
        _followService = followService;
        _userService = userService;
    }

    [Authorize]
    [HttpGet("getFollower/{userId}")]
    public IActionResult GetAllFollowersForUser(int userId)
    {
        var followers = _followService.GetAllFollowersForUser(userId);
        return Ok(followers);
    }

    [AllowAnonymous]
    [HttpGet("getFollowersNb/{userId}")]
    public IActionResult GetAllFollowersNbForUser(int userId)
    {
        var followers = _followService.GetAllFollowersForUser(userId);
        return Ok(followers.Count());
    }

    [AllowAnonymous]
    [HttpGet("getFollowingNb/{userId}")]
    public IActionResult GetAllFollowingNb(int userId)
    {
        var following = _followService.GetAllFollowing(userId);
        return Ok(following.Count());
    }

    [Authorize]
    [HttpGet("getAllFollowing")]
    public IActionResult GetAllFollowing([FromQuery] int id, [FromQuery] int pageNb, [FromQuery] int pageSize,
        [FromQuery] string? searchTerm)
    {
        Console.WriteLine(searchTerm);
        var following = _followService.GetAllFollowingPage(id, pageNb, pageSize, searchTerm);
        return Ok(following);
    }

    [Authorize]
    [HttpDelete]
    public IActionResult DeleteLike([FromQuery] int followingId)
    {
        var user = _userService.GetCurrentUser(Request);
        if (user != null)
        {
            var successResponseDto = _followService.DeleteFollow(user.Id, followingId);
            return Ok(successResponseDto);
        }

        return BadRequest();
    }


    [Authorize]
    [HttpPost]
    public IActionResult AddFollower([FromQuery] int followingId)
    {
        var user = _userService.GetCurrentUser(Request);
        if (user != null)
        {
            var successResponseDto = _followService.AddFollow(user, followingId);
            return Ok(successResponseDto);
        }

        return BadRequest();
    }

    [Authorize]
    [HttpGet]
    public IActionResult IsUserFollow([FromQuery] int followingId)
    {
        var user = _userService.GetCurrentUser(Request);
        if (user != null)
        {
            var value = _followService.IsUserFollowing(user.Id, followingId);
            return Ok(value);
        }

        return BadRequest();
    }
}