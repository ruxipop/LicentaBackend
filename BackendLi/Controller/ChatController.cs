using BackendLi.DataAccess;
using BackendLi.DTOs;
using BackendLi.Entities;
using BackendLi.Hubs;
using BackendLi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace BackendLi.Controller;

[Route("api/[controller]")]
[ApiController]
public class ChatController:ControllerBase
{   private readonly IRepository _repository;
  private readonly IChatService _chatService;
  public ChatController(IRepository repository,IChatService chatService)
  {
    _repository = repository;
    _chatService = chatService;
  }

  [HttpPost("addSenderMessage")]
  [AllowAnonymous]
  public IActionResult addSenderMessage([FromBody] MessageDto messageDto)
  {
    Console.WriteLine("sal");
    var message = new ChatMessageSender()
    {
      SenderId = Convert.ToInt32(messageDto.senderId),
      ReceiverId =Convert.ToInt32(messageDto.receiverId),
      Message = messageDto.Text,
      Timestamp = DateTime.Now,
    };
    using (IUnitOfWork unitOfWork = _repository.CreateUnitOfWork())
    {
            
      unitOfWork.Add(message);
      unitOfWork.SaveChanges();
    }

    return Ok();
  }


  [HttpGet("senderMessage")]
  [AllowAnonymous]
  public IActionResult GetSenderMessage([FromQuery] int senderId, int receiverId)
  {
    return Ok(_chatService.GetSenderMessage(senderId, receiverId));
  }
  
  
  [HttpGet("receiverMessage")]
  [AllowAnonymous]
  public IActionResult GetReceiverMessage([FromQuery] int senderId, int receiverId)
  {
    return Ok(this._chatService.GetReceiverMessage(senderId, receiverId));
  }

  [HttpGet("getChat/{id}")]
  [AllowAnonymous]
  public IActionResult GetChats(int id)
  {
    return Ok(_chatService.GetUsers(id));
  }
}