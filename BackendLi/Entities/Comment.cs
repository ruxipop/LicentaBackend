using System.ComponentModel.DataAnnotations.Schema;

namespace BackendLi.Entities;

[Table("comment")]
public class Comment
{
    public int Id { get; set; }


    [ForeignKey("UserId")] public int UserId { get; set; }

    public virtual User? User { get; set; }
    [ForeignKey("ImageId")] public int ImageId { get; set; }
    public virtual Photo? Image { get; set; }

    public string CommentText { get; set; }
    public DateTime CreatedAt { get; set; }
}