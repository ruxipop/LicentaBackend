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

    [AllowAnonymous]
    [HttpGet]
    public IActionResult GetAllStatistics([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        Console.WriteLine("sa");
        return Ok(_statisticsService.GetNumberOfUsers(startDate, endDate));
    }

    [AllowAnonymous]
    [HttpGet("images")]
    public IActionResult GetAllStatisticsImages([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        Console.WriteLine("sas");
        return Ok(_statisticsService.GetNumberOfPhotos(startDate, endDate));
    }


    
    [AllowAnonymous]
    [HttpGet("allRegisteredUsersNb")]
    public IActionResult GetAllRegisteredUsers()
    {

        return Ok(_statisticsService.GetAllUsersNb());

    }
    
    [AllowAnonymous]
    [HttpGet("allRegisteredUsers")]
    public IActionResult GetAllRegisteredUsers([FromQuery] int pageNb,[FromQuery] int pageSize)
    {

        return Ok(_statisticsService.GetAllUsers(pageNb,pageSize));

    }
    
    [AllowAnonymous]
    [HttpGet("newlyRegisteredUsersNb")]
    public IActionResult GetNewUserNb()
    {

        return Ok(_statisticsService.GetNewUserNb());

    }
    
    [AllowAnonymous]
    [HttpGet("newlyRegisteredUsers")]
    public IActionResult GetNewUsers([FromQuery] int pageNb,[FromQuery] int pageSize)
    {

        return Ok(_statisticsService.GetNewUsers(pageNb,pageSize));

    }
    
    [AllowAnonymous]
    [HttpGet("newlyUploadedImages")]
    public IActionResult GetNewImages([FromQuery] int pageNb,[FromQuery] int pageSize)
    {

        return Ok(_statisticsService.GetNewImages(pageNb,pageSize));

    }
    
    [AllowAnonymous]
    [HttpGet("allUploadedImages")]
    public IActionResult GetAllUploadedImages([FromQuery] int pageNb,[FromQuery] int pageSize)
    {

        return Ok(_statisticsService.GetAllImages(pageNb,pageSize));

    }

    [AllowAnonymous]
    [HttpGet("getStatisticsByType")]
    public IActionResult GetStatisticsByType()
    {
        return Ok(_statisticsService.GetStatisticsByType());
    }
    
    [AllowAnonymous]
    [HttpGet("getStatisticsByCategory")]
    public IActionResult GetStatisticsByCategory()
    {
        return Ok(_statisticsService.GetStatisticsByCategory());
    }
    
    [AllowAnonymous]
    [HttpGet("getFirstImages")]
    public IActionResult GetFirst()
    {
        return Ok(_statisticsService.GetFirstImages());
    }
}