using BackendLi.DTOs;
using BackendLi.Entities;
using BackendLi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackendLi.Controller;

[Route("api/[controller]")]
[ApiController]
public class ReportController:ControllerBase
{
    private readonly IReportService _reportService;


    public ReportController(IReportService reportService)
    {
        _reportService = reportService;
    }
    

    [HttpGet]
    [Authorize(Roles = "EDITOR")]
    public IEnumerable<Report> GetReports()
    {
        return _reportService.GetReports();
    }
    
    [HttpGet("exist/{imageId}")]
    [Authorize(Roles = "EDITOR")]
    public bool GetExist([FromRoute] int imageId)
    {
        return _reportService.existReport(imageId);
        
    }
    
    [HttpDelete("{reportId}")]
    [Authorize(Roles = "EDITOR")]
    public void DeleteReport([FromRoute] int reportId)
    {
        _reportService.DeleteReport(reportId);
    }
    [HttpDelete("deleteReportImage/{imageId}")]
    [Authorize(Roles = "EDITOR")]
    public void DeleteReportImage([FromRoute] int imageId)
    {
        _reportService.DeleteReportImage(imageId);
    }

    
    [HttpPost("sent-email")]
    [AllowAnonymous]
    public IActionResult Create([FromBody] ReportEmailDto reportEmailDto)
    {
        Console.WriteLine("Da");
      _reportService.CreateReportEmail(reportEmailDto);

        return Ok(new SuccessResponseDto("The email was sent successfully."));
    }
    
    [HttpPost]
    [AllowAnonymous]
    public IActionResult CreateReport([FromBody] Report report)
    {
        Console.WriteLine("Da");
        _reportService.CreateReport(report);

        return Ok(new SuccessResponseDto("The report was created successfully."));
    }
}