using BackendLi.DTOs;
using BackendLi.Entities;

namespace BackendLi.Services;

public interface INotificationService
{

    public IEnumerable<Notification> GetAllNotificationByUserId(int userId, int pageNb, int pageSize);
    public SuccessResponseDto DeleteAllNotifications(int userId);
    public SuccessResponseDto DeleteNotification(int notificationId);


}