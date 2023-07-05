using BackendLi.DTOs;
using BackendLi.Entities;
using BackendLi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackendLi.Controller;


[Route("api/[controller]")]
[ApiController]
public class CommentController:ControllerBase
{
    private readonly ICommentService _commentService;

    public CommentController(ICommentService commentService)
    {
        _commentService = commentService;
    }

    
    [HttpPost("addComment")]
    [AllowAnonymous]
    public void AddComment([FromBody] Comment comment)
    {
        Console.WriteLine("cc"+comment.CreatedAt);
        Console.WriteLine("intra in add comment" +comment);
        _commentService.AddComment(comment);
        // return Ok();
    }
    
    [AllowAnonymous]
    [HttpGet("{imageId}")]
    public IEnumerable<Comment> GetComments(int imageId)
    {

        return _commentService.GetComments(imageId);
    }
}