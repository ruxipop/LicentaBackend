using BackendLi.DTOs;
using BackendLi.Entities;
using BackendLi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackendLi.Controller;

[Route("api/[controller]")]
[ApiController]
public class NotificationController : ControllerBase
{
    public readonly IFollowService _followService;
    public readonly INotificationService _notificationService;

    public NotificationController(INotificationService notificationService, IFollowService followService)
    {
        _notificationService = notificationService;
        _followService = followService;
    }

    [Authorize]
    [HttpGet]
    public IActionResult GetNotificationById([FromQuery] int userId, [FromQuery] int pageNb, [FromQuery] int pageSize)
    {
        var notificationsDto = new List<NotificationDto>();
        var allNotification = _notificationService.GetAllNotificationByUserId(userId, pageNb, pageSize);
        foreach (var notification in allNotification)
            if (notification.Type == NotificationType.FOLLOW)
                notificationsDto.Add(new NotificationDto(_followService.IsUserFollowing(userId, notification.SenderId),
                    notification));
            else
                notificationsDto.Add(new NotificationDto(null, notification));

        return Ok(notificationsDto);
    }


    [Authorize]
    [HttpDelete("{userId}")]
    public IActionResult DeleteNotifications(int userId)
    {
        var successResponseDto = _notificationService.DeleteAllNotifications(userId);

        return Ok(successResponseDto);
    }

    [Authorize]
    [HttpDelete("delete/{notificationId}")]
    public IActionResult DeleteNotification(int notificationId)
    {
        var successResponseDto = _notificationService.DeleteNotification(notificationId);

        return Ok(successResponseDto);
    }
}