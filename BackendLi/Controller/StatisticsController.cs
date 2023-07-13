using BackendLi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackendLi.Controller;

[Route("api/[controller]")]
[ApiController]
public class StatisticsController : ControllerBase
{
    public readonly IStatisticsService _statisticsService;

    public StatisticsController(IStatisticsService statisticsService)
    {
        _statisticsService = statisticsService;
    }

    [Authorize(Roles = "EDITOR")]
    [HttpGet]
    public IActionResult GetAllStatistics([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        return Ok(_statisticsService.GetNumberOfUsers(startDate, endDate));
    }

    [Authorize(Roles = "EDITOR")]
    [HttpGet("images")]
    public IActionResult GetAllStatisticsImages([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        return Ok(_statisticsService.GetNumberOfPhotos(startDate, endDate));
    }

    [Authorize(Roles = "EDITOR")]
    [HttpGet("allImagesNb")]
    public IActionResult GetAllImagesNb()
    {
        return Ok(_statisticsService.GetAllImagesNb());
    }
    
    [Authorize(Roles = "EDITOR")]
    [HttpGet("allNewImagesNb")]
    public IActionResult GetNewImagesNb()
    {
        return Ok(_statisticsService.GetNewImagesNb());
    }

    [Authorize(Roles = "EDITOR")]
    [HttpGet("allRegisteredUsersNb")]
    public IActionResult GetAllRegisteredUsers()
    {
        return Ok(_statisticsService.GetAllUsersNb());
    }

    [Authorize(Roles = "EDITOR")]
    [HttpGet("allRegisteredUsers")]
    public IActionResult GetAllRegisteredUsers([FromQuery] int pageNb, [FromQuery] int pageSize)
    {
        return Ok(_statisticsService.GetAllUsers(pageNb, pageSize));
    }

    [Authorize(Roles = "EDITOR")]
    [HttpGet("newlyRegisteredUsersNb")]
    public IActionResult GetNewUserNb()
    {
        return Ok(_statisticsService.GetNewUserNb());
    }

    [Authorize(Roles = "EDITOR")]
    [HttpGet("newlyRegisteredUsers")]
    public IActionResult GetNewUsers([FromQuery] int pageNb, [FromQuery] int pageSize)
    {
        return Ok(_statisticsService.GetNewUsers(pageNb, pageSize));
    }

    [Authorize(Roles = "EDITOR")]
    [HttpGet("newlyUploadedImages")]
    public IActionResult GetNewImages([FromQuery] int pageNb, [FromQuery] int pageSize)
    {
        return Ok(_statisticsService.GetNewImages(pageNb, pageSize));
    }

    [Authorize(Roles = "EDITOR")]
    [HttpGet("allUploadedImages")]
    public IActionResult GetAllUploadedImages([FromQuery] int pageNb, [FromQuery] int pageSize)
    {
        return Ok(_statisticsService.GetAllImages(pageNb, pageSize));
    }

    [Authorize(Roles = "EDITOR")]
    [HttpGet("getStatisticsByType")]
    public IActionResult GetStatisticsByType()
    {
        return Ok(_statisticsService.GetStatisticsByType());
    }

    [Authorize(Roles = "EDITOR")]
    [HttpGet("getStatisticsByCategory")]
    public IActionResult GetStatisticsByCategory()
    {
        return Ok(_statisticsService.GetStatisticsByCategory());
    }

    [Authorize(Roles = "EDITOR")]
    [HttpGet("getFirstImages")]
    public IActionResult GetFirst()
    {
        return Ok(_statisticsService.GetFirstImages());
    }
}