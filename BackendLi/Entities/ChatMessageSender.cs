using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendLi.Entities;

[Table("chat_sender")]
public class ChatMessageSender
{
    [Key] public int Id { get; set; }

    [ForeignKey("Sender")] public int SenderId { get; set; }

    public virtual User? Sender { get; set; }

    [ForeignKey("Receiver")] public int ReceiverId { get; set; }

    public virtual User? Receiver { get; set; }

    [Required] public string Message { get; set; }

    public DateTime Timestamp { get; set; }

    public MessageStatus Status { get; set; }
}