using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackendLi.DataAccess;
using BackendLi.Entities;
using Microsoft.AspNetCore.SignalR;

namespace BackendLi.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IRepository _repository;

        private static Dictionary<string, string> Users = new Dictionary<string, string>();

        public ChatHub(IRepository repository)
        {
            _repository = repository;
        }
        public override async Task OnConnectedAsync()
        {
            Console.WriteLine("intra");
            string username = Context.GetHttpContext().Request.Query["username"];
            Users.Add(Context.ConnectionId, username);
            Console.WriteLine("username " + username);
            Console.WriteLine(Context.ConnectionId);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            string username = Users.FirstOrDefault(u => u.Key == Context.ConnectionId).Value;
            Console.WriteLine(username);
            if (!string.IsNullOrEmpty(username))
            {
                Users.Remove(Context.ConnectionId); // Eliminați utilizatorul din dicționar

                Console.WriteLine("User disconnected: " + username);
                // await AddMessageToChat(string.Empty, $"{username} left!");
            }

            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendNotification(string receiverId,string senderId, string message)
        {
            Console.WriteLine("oo "+Convert.ToInt32(receiverId)+" "+Convert.ToInt32(senderId)+" "+message+" "+ MessageStatus.DELIVERED + DateTime.Now);
            if (senderId == null || receiverId == null || message == null)
            {
                // Tratați cazul în care una dintre variabile este nulă
                // De exemplu, puteți arunca o excepție, puteți trimite un mesaj de eroare sau puteți ignora acțiunea
                return;
            }
            var messageDto = new ChatMessage
            {
                SenderId = Convert.ToInt32(senderId),
                ReceiverId =Convert.ToInt32(receiverId),
                Message = message,
                Timestamp = DateTime.Now,
                Status = MessageStatus.DELIVERED
            };
            Console.WriteLine("Ceeee");
            using (IUnitOfWork unitOfWork = _repository.CreateUnitOfWork())
            {
            
                unitOfWork.Add(messageDto);
                unitOfWork.SaveChanges();
            }
            Console.WriteLine("Ceeee");

            string connectionId = Users.FirstOrDefault(u => u.Value == receiverId).Key;
            Console.WriteLine(receiverId);
            Console.WriteLine(connectionId);
            await Clients.Client(connectionId).SendAsync("ReceiveNotification",messageDto);
            // await Clients.All.SendAsync("ReceiveNotification",senderId, message);

        }
    }
}