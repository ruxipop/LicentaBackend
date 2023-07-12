using BackendLi.DataAccess;
using BackendLi.DTOs;
using BackendLi.Entities;
using BackendLi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackendLi.Controller;

[Route("api/[controller]")]
[ApiController]
public class ChatController : ControllerBase
{
    private readonly IChatService _chatService;
    private readonly IRepository _repository;

    public ChatController(IRepository repository, IChatService chatService)
    {
        _repository = repository;
        _chatService = chatService;
    }

    [HttpPost("addSenderMessage")]
    [Authorize]
    public IActionResult addSenderMessage([FromBody] MessageDto messageDto)
    {
        var message = new ChatMessageSender
        {
            SenderId = Convert.ToInt32(messageDto.senderId),
            ReceiverId = Convert.ToInt32(messageDto.receiverId),
            Message = messageDto.Text,
            Timestamp = DateTime.Now
        };
        using (var unitOfWork = _repository.CreateUnitOfWork())
        {
            unitOfWork.Add(message);
            unitOfWork.SaveChanges();
        }

        return Ok();
    }


    [HttpGet("senderMessage")]
    [Authorize]
    public IActionResult GetSenderMessage([FromQuery] int senderId, int receiverId)
    {
        return Ok(_chatService.GetSenderMessage(senderId, receiverId));
    }


    [HttpGet("receiverMessage")]
    [Authorize]
    public IActionResult GetReceiverMessage([FromQuery] int senderId, int receiverId)
    {
        return Ok(_chatService.GetReceiverMessage(senderId, receiverId));
    }

    [HttpGet("getChat/{id}")]
    [Authorize]
    public IActionResult GetChats(int id)
    {
        return Ok(_chatService.GetUsers(id));
    }
}