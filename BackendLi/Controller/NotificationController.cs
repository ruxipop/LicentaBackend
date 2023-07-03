using BackendLi.DTOs;
using BackendLi.Entities;
using BackendLi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace BackendLi.Controller;


[Route("api/[controller]")]
[ApiController]
public class NotificationController:ControllerBase
{
    public readonly INotificationService _notificationService;
    public readonly IFollowService _followService;

    public NotificationController(INotificationService notificationService,IFollowService followService)
    {
        _notificationService = notificationService;
        _followService = followService;
    }
    
    [AllowAnonymous]
    [HttpGet]
    public IActionResult GetNotificationById([FromQuery] int userId,[FromQuery] int pageNb, [FromQuery] int pageSize)
    {
        List<NotificationDto> notificationsDto = new List<NotificationDto>();
        var allNotification = _notificationService.GetAllNotificationByUserId(userId, pageNb, pageSize);
        foreach (var notification in allNotification)
        {
            if (notification.Type == NotificationType.FOLLOW)
            {
                notificationsDto.Add(new NotificationDto(_followService.IsUserFollowing(userId,notification.SenderId),notification));
            }
            else
            {
                notificationsDto.Add(new NotificationDto(null,notification));
            }
            
        }

        return Ok(notificationsDto);
        
    }
    
    
    
    [AllowAnonymous]
    [HttpDelete("{userId}")]
    public IActionResult DeleteNotifications(int userId)
    {
Console.WriteLine("c");

        SuccessResponseDto successResponseDto = _notificationService.DeleteAllNotifications(userId);
  
            return Ok(successResponseDto);
        


    }
    
    [AllowAnonymous]
    [HttpDelete("delete/{notificationId}")]
    public IActionResult DeleteNotification(int notificationId)
    {
        SuccessResponseDto successResponseDto = _notificationService.DeleteNotification(notificationId);
  
        return Ok(successResponseDto);
        
    }
}