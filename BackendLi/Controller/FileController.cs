using BackendLi.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackendLi.Controller;

[Route("api/[controller]")]
[ApiController]
public class FileController:ControllerBase
{
    public FileController()
    {
    }

    [AllowAnonymous]
    [HttpPost("addPublicKey")]
    public IActionResult SavePublicKey([FromBody]FileRequestDto fileRequestDto)
    {
        string name=fileRequestDto.Name;
        string text = fileRequestDto.Content;
        
        string filePath = "publicKeys/public_key_"+name+".txt";
        using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
        {
            using (StreamWriter writer = new StreamWriter(fileStream))
            {
                writer.Write(text);
            }
        }
        return Ok();
    }
    
    
    [AllowAnonymous]
    [HttpPost("addSecretKey")]
    public IActionResult SaveSecretKey([FromBody]FileRequestDto fileRequestDto)
    {
        string name=fileRequestDto.Name;
        string text = fileRequestDto.Content;
        
        string filePath = "secretKeys/secret_key_"+name+".txt";
        using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
        {
            using (StreamWriter writer = new StreamWriter(fileStream))
            {
                writer.Write(text);
            }
        }
        return Ok();
    }
    
    [AllowAnonymous]
    [HttpGet("readPublicKey/{name}")]
    public IActionResult ReadPublicKey(string name)
    {
        string filePath = "publicKeys/public_key_" + name + ".txt";
        string content = string.Empty;

        if (System.IO.File.Exists(filePath))
        {
            using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
            {
                using (StreamReader reader = new StreamReader(fileStream))
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
        string filePath = "secretKeys/secret_key_" + name + ".txt";
        string content = string.Empty;

        if (System.IO.File.Exists(filePath))
        {
            using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
            {
                using (StreamReader reader = new StreamReader(fileStream))
                {
                    content = reader.ReadToEnd();
                }
            }
            Console.WriteLine(content);
            return Ok(content);
        }
    
        return NotFound();
        
    }
    
}