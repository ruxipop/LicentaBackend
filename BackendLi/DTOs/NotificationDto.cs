using BackendLi.Entities;

namespace BackendLi.DTOs;

public class NotificationDto
{
    public NotificationDto(bool? isFollowing, Notification notification)
    {
        IsFollowing = isFollowing;
        Notification = notification;
    }

    public bool? IsFollowing { set; get; }
    public Notification Notification { set; get; }
}