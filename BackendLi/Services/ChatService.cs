using BackendLi.DataAccess;
using BackendLi.Entities;
using BackendLi.Entities.Attributes;
using Microsoft.EntityFrameworkCore;

namespace BackendLi.Services;

[Service(typeof(IChatService))]

public class ChatService:IChatService
{    private readonly IRepository _repository;


    public ChatService(IRepository repository)
    {
        _repository = repository;
    }

    public  IEnumerable<ChatMessageSender> GetSenderMessage(int senderId, int receiverId)
    {
        return _repository.GetEntities<ChatMessageSender>()
            .Where(x => x.SenderId == senderId && x.ReceiverId == receiverId).ToList();
    }
    
    
    public  IEnumerable<ChatMessage> GetReceiverMessage(int senderId, int receiverId)
    {
        return _repository.GetEntities<ChatMessage>()
            .Where(x => x.SenderId == senderId && x.ReceiverId == receiverId).ToList();
    }

    public List<User>? GetUsers(int id)
    {
        List<User?> userList = _repository.GetEntities<ChatMessage>()
            .Where(x => x.SenderId == id || x.ReceiverId == id)
            .Select(x => id == x.SenderId ? x.Receiver : x.Sender)
            .ToList();

        List<User?> distinctUsers = userList
            .Where(user => user != null)
            .Distinct(new UserEqualityComparer())
            .AsEnumerable()
            .ToList();


    Console.WriteLine(distinctUsers.Count);
   

        return distinctUsers;
    }



    public class UserEqualityComparer : IEqualityComparer<User?>
    {
        public bool Equals(User? x, User? y)
        {
            if (ReferenceEquals(x, y))
                return true;

            if (x is null || y is null)
                return false;

            return x.Id == y.Id;
        }

        public int GetHashCode(User? obj)
        {
            return obj?.Id ?? 0;
        }
    }

}