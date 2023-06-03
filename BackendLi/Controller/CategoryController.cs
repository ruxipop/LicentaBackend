using BackendLi.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackendLi.Controller;

[Route("api/[controller]")]
[ApiController]
public class CategoryController:ControllerBase
{
    [AllowAnonymous]
    [HttpGet]
   
    public IActionResult GetAllCategories()
    {
        var t = Enum.GetNames(typeof(ImageCategory));
        return Ok(t);
    }

}