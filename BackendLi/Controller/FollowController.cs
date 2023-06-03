using BackendLi.DTOs;
using BackendLi.Entities;
using BackendLi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackendLi.Controller;

[Route("api/[controller]")]
[ApiController]
public class FollowController:ControllerBase
{
    private readonly IFollowService _followService;
    private readonly IUserService _userService;
    public FollowController(IFollowService followService,IUserService userService)
    {
        _followService = followService;
        _userService = userService;
    }

    [AllowAnonymous]
    [HttpGet("getFollower/{userId}")]

    public IActionResult GetAllFollowersForUser(int userId)
    {
        var followers = _followService.GetAllFollowersForUser(userId);
        return Ok(followers);
    }

    
    // [AllowAnonymous]
    // [HttpGet("getFollowing/{userId}")]
    // public IActionResult GetAllFollowing(int userId)
    // {
    //     var following = _followService.GetAllFollowing(userId);
    //     return Ok(following);
    // }
    
    [Authorize]
    [HttpDelete]
    public IActionResult DeleteLike([FromQuery] int followingId)
    {
      
        Console.WriteLine("follow remove");

        
        User? user = _userService.GetCurrentUser(Request);
        if (user != null)
        {
            SuccessResponseDto successResponseDto = _followService.DeleteFollow(user.Id, followingId);
            Console.WriteLine(followingId);
            Console.WriteLine(user.Id);
            return Ok(successResponseDto);
        }

        return BadRequest();

    }

    [Authorize]
    [HttpPost]
    public IActionResult AddFollower([FromQuery] int followingId)
    {
        Console.WriteLine("follow add");
      
        User? user = _userService.GetCurrentUser(Request);
        if (user != null)
        {
            SuccessResponseDto successResponseDto = _followService.AddFollow(user, followingId);
            return Ok(successResponseDto);
        }

        return BadRequest();

    }
    
    [Authorize]
    [HttpGet]
    public IActionResult IsUserFollow([FromQuery] int followingId)
    {
        Console.WriteLine("follow add");
      
        User? user = _userService.GetCurrentUser(Request);
        if (user != null)
        {
            bool value = _followService.IsUserFollowing(user.Id, followingId);
            return Ok(value);
        }

        return BadRequest();

    }
    
}