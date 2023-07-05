using BackendLi.DataAccess;
using BackendLi.DTOs;
using BackendLi.Entities;
using BackendLi.Entities.Attributes;
using Microsoft.EntityFrameworkCore;

namespace BackendLi.Services;

[Service(typeof(INotificationService))]

public class NotificationService:INotificationService
{
    private readonly IRepository _repository;


    public NotificationService(IRepository repository)
    {
        _repository = repository;
    }


    public IEnumerable<Notification> GetAllNotificationByUserId(int userId,int pageNb,int pageSize)
    {
        return  _repository.GetEntities<Notification>().Where(l=>l.ReceiverId==userId).Include(i=>i.Sender )  .OrderByDescending(n => n.Timestamp).Skip((pageNb - 1) * pageSize).Take(pageSize)
           
            .ToList();
        
    }

    public SuccessResponseDto DeleteAllNotifications(int userId)
    {

        using (IUnitOfWork unitOfWork = _repository.CreateUnitOfWork())
        {
            var notifications = _repository.GetEntities<Notification>().Where(n => n.ReceiverId == userId).ToList();
                     
            if (notifications.Count!=0)
            {
                unitOfWork.DeleteRange(notifications);
                unitOfWork.SaveChanges();
                return new SuccessResponseDto("Notifications deleted successfully.");
            }

        }
        return new SuccessResponseDto("No notifications");

    }

    public SuccessResponseDto DeleteNotification(int notificationId)
    {

        using (IUnitOfWork unitOfWork = _repository.CreateUnitOfWork())
        {
            var notification = _repository.GetEntities<Notification>().FirstOrDefault(n => n.Id == notificationId);
                     
            if (notification!=null)
            {
                unitOfWork.Delete(notification);
                unitOfWork.SaveChanges();
                return new SuccessResponseDto("Notification deleted successfully.");
            }

        }
        return new SuccessResponseDto("No notification");

    }

    public void createNotification(Notification notification)
    {
        using (IUnitOfWork unitOfWork = _repository.CreateUnitOfWork())
        {
            
            unitOfWork.Add(notification);
            unitOfWork.SaveChanges();
        }

    }


}