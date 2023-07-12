using BackendLi.Entities;

namespace BackendLi.DTOs;

public class UserChatDto
{
    public UserChatDto(User user, DateTime lastMessage)
    {
        User = user;
        LastMessage = lastMessage;
    }

    public User User { get; set; }
    public DateTime LastMessage { get; set; }
}