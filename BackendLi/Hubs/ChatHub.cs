using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Threading.Tasks;
using BackendLi.DataAccess;
using BackendLi.Entities;
using BackendLi.Services;
using Microsoft.AspNetCore.SignalR;

namespace BackendLi.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IRepository _repository;
        private readonly INotificationService _notification;

        private static Dictionary<string, string> Users = new Dictionary<string, string>();

        public ChatHub(IRepository repository,INotificationService notificationService)
        {
            _repository = repository;
            _notification = notificationService;
        }
        public override async Task OnConnectedAsync()
        {
            string username = Context.GetHttpContext().Request.Query["username"];
            Users.Add(Context.ConnectionId, username);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
             string username = Users.FirstOrDefault(u => u.Key == Context.ConnectionId).Value;
            if (!string.IsNullOrEmpty(username))
            {
                Users.Remove(Context.ConnectionId);
            }

            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendChat(string receiverId,string senderId, string message)
        {
           
            var messageDto = new ChatMessage
            {
                SenderId = Convert.ToInt32(senderId),
                ReceiverId =Convert.ToInt32(receiverId),
                Message = message,
                Timestamp = DateTime.Now,
                Status = MessageStatus.DELIVERED
            };
            using (IUnitOfWork unitOfWork = _repository.CreateUnitOfWork())
            {
            
                unitOfWork.Add(messageDto);
                unitOfWork.SaveChanges();
            }

            string connectionId = Users.FirstOrDefault(u => u.Value == receiverId).Key;
            if (connectionId != null)
            {
                await Clients.Client(connectionId).SendAsync("ReceiveChat", messageDto);
            }
        }


        public async Task SendNotification(string receiverId, string senderId, string? imageId, NotificationType type)
        {
            if (senderId!=receiverId)
            {
                var notificationDto = new Notification()
                {
                    SenderId = Convert.ToInt32(senderId),
                    ReceiverId = Convert.ToInt32(receiverId),
                    ImageId = imageId ==null? null: Convert.ToInt32(imageId),
                    Timestamp = DateTime.Now,
                    Type = type
                };
                _notification.CreateNotification(notificationDto);
                string connectionId = Users.FirstOrDefault(u => u.Value == receiverId).Key;
                if (connectionId != null)
                {
                    await Clients.Client(connectionId).SendAsync("ReceiveNotification", notificationDto);
                }

            }
        }

    }
}