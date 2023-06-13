using System.ComponentModel.DataAnnotations.Schema;

namespace BackendLi.Entities;


[Table("gallery")]
public class Gallery
{
    public int Id { get; set; }
    
    public string Name { get; set; }

    [ForeignKey("User")]
    public int? UserId { get; set; }
    public virtual User? User { get; set; }

    public string Description { get; set; }
    public List<Image>? Images{ get; set; } 
    
    public bool IsPrivate { get; set; }

    public Gallery()
    {
        Images = new List<Image>();
    }

    
}