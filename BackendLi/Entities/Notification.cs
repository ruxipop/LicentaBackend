using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendLi.Entities;

[Table("notification")]

public class Notification
{
    [Key]
    public int Id { get; set; }
    
    public NotificationType Type { get; set; }
    
    public string Content { get; set; }
  
    
    [ForeignKey("Sender")]
    public int SenderId { get; set; }
    public virtual User? Sender { get; set; }
        
    [ForeignKey("Receiver")]
    public int ReceiverId { get; set; }
    public virtual User? Receiver { get; set; }
    
    public DateTime Timestamp { get; set; }

}