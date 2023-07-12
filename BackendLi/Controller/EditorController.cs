using BackendLi.Entities;
using BackendLi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackendLi.Controller;

[Route("api/[controller]")]
[ApiController]
public class EditorController : ControllerBase
{
    private readonly IEditorService _editorService;

    public EditorController(IEditorService editorService)
    {
        _editorService = editorService;
    }

    [HttpGet]
    [Authorize(Roles = "EDITOR")]
    public IEnumerable<EditorChoice> GetImagesByEditor([FromQuery] int userId, [FromQuery] int pageNb,
        [FromQuery] int pageSize)
    {
        return _editorService.GetVotedImages(userId, pageNb, pageSize);
    }


    [HttpPost]
    [Authorize(Roles = "EDITOR")]
    public IActionResult AddVoteToImage([FromBody] EditorChoice editorChoice)
    {
        var successResponseDto = _editorService.AddImagedToVoted(editorChoice);
        return Ok(successResponseDto);
    }

    [Authorize(Roles = "EDITOR")]
    [HttpDelete("{id}")]
    public IActionResult DeleteVote(int id)
    {
        var successResponseDto = _editorService.DeleteVote(id);
        return Ok(successResponseDto);
    }

    [Authorize(Roles = "EDITOR")]
    [HttpGet("editorId")]
    public IActionResult GetEditorByIds([FromQuery] int userId, [FromQuery] int imageId)
    {
        return Ok(_editorService.GetEditorChoice(userId, imageId));
    }


    [Authorize(Roles = "EDITOR")]
    [HttpGet("voted/{imageId}")]
    public IActionResult IsImageVoted(int imageId)
    {
        return Ok(_editorService.IsImageVoted(imageId));
    }

    [Authorize(Roles = "EDITOR")]
    [HttpGet("nbOfVotes/{userId}")]
    public IActionResult NbOfVotes(int userId)
    {
        return Ok(_editorService.GetNbOfVotes(userId));
    }
}