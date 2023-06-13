using System.ComponentModel.DataAnnotations.Schema;
using BackendLi.Services;

namespace BackendLi.Entities;



[Table("image")]
public class Image
{
    
    public int Id { get; set; }
    public int AutorId { get; set; } 

    [ForeignKey("AutorId")] 
    public User Autor { get; set; }

    public string Description { get; set; }
    public string Title { get; set; }

    public DateTime Taked{ get; set; }
    public DateTime Uploaded { get; set; }
    [ForeignKey("LocationId")] 

    public int? LocationId { get; set; }
    
    public Location? Location { get; set; }
    public string ImageUrl { get; set; }
    public ImageCategory Category { get; set; } 
    public int Width { get; set; }
    public int Height { get; set; }
    public EditorChoice? EditorChoice { get; set; }


    public List<Like> Likes { get; set; }
    
    
    [ForeignKey("Gallery")]
    public int? GalleryId { get; set; }
    public virtual Gallery? Gallery { get; set; }


    public ICollection<Comment> Comments { get; set; }

    public Image()
    {
        Comments= new HashSet<Comment>();
        Likes = new List<Like>();

    }
    

    
}