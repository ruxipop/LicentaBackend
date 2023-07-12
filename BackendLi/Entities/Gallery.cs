using System.ComponentModel.DataAnnotations.Schema;

namespace BackendLi.Entities;

[Table("gallery")]
public class Gallery
{
    public Gallery()
    {
        Images = new List<Photo>();
    }

    public int Id { get; set; }

    public string Name { get; set; }

    [ForeignKey("User")] public int? UserId { get; set; }

    public virtual User? User { get; set; }

    public bool IsPrivate { get; set; }

    public string Description { get; set; }

    public List<Photo>? Images { get; set; }
}