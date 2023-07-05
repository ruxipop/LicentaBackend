using BackendLi.DTOs;
using BackendLi.Entities;

namespace BackendLi.Services;

public interface IChatService
{
    public IEnumerable<ChatMessageSender> GetSenderMessage(int senderId, int receiverId);
    public IEnumerable<ChatMessage> GetReceiverMessage(int senderId, int receiverId);
    public List<UserChatDto>? GetUsers(int id);

}