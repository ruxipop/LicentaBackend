using BackendLi.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackendLi.Controller;

[Route("api/[controller]")]
[ApiController]
public class FileController : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("addPublicKey")]
    public IActionResult SavePublicKey([FromBody] FileRequestDto fileRequestDto)
    {
        var name = fileRequestDto.Name;
        var text = fileRequestDto.Content;

        var filePath = "publicKeys/public_key_" + name + ".txt";
        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            using (var writer = new StreamWriter(fileStream))
            {
                writer.Write(text);
            }
        }

        return Ok();
    }


    [AllowAnonymous]
    [HttpPost("addSecretKey")]
    public IActionResult SaveSecretKey([FromBody] FileRequestDto fileRequestDto)
    {Console.WriteLine("Ce");
        var name = fileRequestDto.Name;
        var text = fileRequestDto.Content;

        var filePath = "secretKeys/secret_key_" + name + ".txt";
        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            using (var writer = new StreamWriter(fileStream))
            {
                writer.Write(text);
            }
        }

        return Ok();
    }

    [Authorize]
    [HttpGet("readPublicKey/{name}")]
    public IActionResult ReadPublicKey(string name)
    {
        var filePath = "publicKeys/public_key_" + name + ".txt";
        var content = string.Empty;

        if (System.IO.File.Exists(filePath))
        {
            using (var fileStream = new FileStream(filePath, FileMode.Open))
            {
                using (var reader = new StreamReader(fileStream))
                {
                    content = reader.ReadToEnd();
                }
            }

            return Ok(content);
        }

        return NotFound();
    }

    [Authorize]
    [HttpGet("readSecretKey/{name}")]
    public IActionResult ReadSecretKey(string name)
    {
        var filePath = "secretKeys/secret_key_" + name + ".txt";
        var content = string.Empty;

        if (System.IO.File.Exists(filePath))
        {
            using (var fileStream = new FileStream(filePath, FileMode.Open))
            {
                using (var reader = new StreamReader(fileStream))
                {
                    content = reader.ReadToEnd();
                }
            }

            return Ok(content);
        }

        return NotFound();
    }
}