using System.ComponentModel.DataAnnotations.Schema;

namespace BackendLi.Entities;


[Table("like")]
public class Like
{
    public int Id { get; set; }
    
    [ForeignKey("UserId")] 
    public int UserId { get; set; } 
    
    public virtual User? User { get; set; }
    [ForeignKey("ImageId")] 
    
    public int ImageId { get; set; }
    public virtual Photo? Image{ get; set; }

    public Like(int userId, int imageId)
    {
        UserId = userId;
        ImageId = imageId;
    }
}