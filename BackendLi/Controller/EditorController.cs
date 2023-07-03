using BackendLi.DTOs;
using BackendLi.Entities;
using BackendLi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace BackendLi.Controller;

[Route("api/[controller]")]
[ApiController]
public class EditorController:ControllerBase
{
    private readonly IEditorService _editorService;

    public EditorController(IEditorService editorService)
    {
        _editorService = editorService;
    }

    [AllowAnonymous]
    [HttpGet]
    public IEnumerable<EditorChoice> GetImagesByEditor([FromQuery] int userId,[FromQuery] int pageNb, [FromQuery] int pageSize)
    {
        return _editorService.GetVotedImages(userId, pageNb, pageSize);
    }
    
    
    [AllowAnonymous]
    [HttpPost]
    public IActionResult AddVoteToImage([FromBody] EditorChoice editorChoice)
    {
      Console.WriteLine("ajun "+editorChoice.ImageId  +" "+editorChoice.EditorId);
       // imageId User? user = _userService.GetCurrentUser(Request);
       //  if (user != null)
       //  {
       var successResponseDto = _editorService.AddImagedToVoted(editorChoice);
            return Ok(successResponseDto);
        // }

        // return BadRequest();

    }

    [AllowAnonymous]
    [HttpDelete("{id}")]
    public IActionResult DeleteVote(int id)
    {

Console.WriteLine("delete" +id);

        SuccessResponseDto successResponseDto = _editorService.DeleteVote(id);
        Console.WriteLine(successResponseDto);
            return Ok(successResponseDto);
   

    }

    [AllowAnonymous]
    [HttpGet("editorId")]
    public IActionResult GetEditorByIds([FromQuery] int userId, [FromQuery] int imageId)
    {
        Console.WriteLine("ajunge "+userId+" "+imageId);
        return Ok(_editorService.GetEditorChoice(userId, imageId));
    }


    [AllowAnonymous]
    [HttpGet("voted/{imageId}")]
    public IActionResult IsImageVoted(int imageId)
    {
        return Ok(_editorService.IsImageVoted(imageId));
    }
    
    [AllowAnonymous]
    [HttpGet("nbOfVotes/{userId}")]
    public IActionResult NbOfVotes(int userId)
    {
        return Ok(_editorService.GetNbOfVotes(userId));
    }
    
}