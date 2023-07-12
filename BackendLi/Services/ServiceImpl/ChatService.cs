using BackendLi.DataAccess;
using BackendLi.DTOs;
using BackendLi.Entities;
using BackendLi.Entities.Attributes;

namespace BackendLi.Services.ServiceImpl;

[Service(typeof(IChatService))]
public class ChatService : IChatService
{
    private readonly IRepository _repository;


    public ChatService(IRepository repository)
    {
        _repository = repository;
    }

    public IEnumerable<ChatMessageSender> GetSenderMessage(int senderId, int receiverId)
    {
        return _repository.GetEntities<ChatMessageSender>()
            .Where(x => x.SenderId == senderId && x.ReceiverId == receiverId).ToList();
    }


    public IEnumerable<ChatMessage> GetReceiverMessage(int senderId, int receiverId)
    {
        return _repository.GetEntities<ChatMessage>()
            .Where(x => x.SenderId == senderId && x.ReceiverId == receiverId).ToList();
    }

    public List<UserChatDto> GetUsers(int id)
    {
        var userList = _repository.GetEntities<ChatMessage>()
            .Where(x => x.SenderId == id || x.ReceiverId == id)
            .Select(x => id == x.SenderId ? x.Receiver : x.Sender)
            .ToList();

        var distinctUsers = userList
            .Where(user => user != null)
            .Distinct(new UserEqualityComparer())
            .AsEnumerable()
            .ToList();


        var chatDTOs = new List<UserChatDto>();
        foreach (var user in distinctUsers)
            chatDTOs.Add(new UserChatDto(user, _repository.GetEntities<ChatMessage>()
                .Where(x => x.SenderId == user.Id || x.ReceiverId == user.Id)
                .Max(x => x.Timestamp)));

        return chatDTOs.OrderByDescending(c => c.LastMessage).ToList();
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