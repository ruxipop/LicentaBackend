using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Mime;

namespace BackendLi.Entities;


[Table("users")]
public class User
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }
    public string Name { get; set; }
    
    public string Username { get; set; }
    
    public string? Description { get; set; }
    public List<Image>? Images{ get; set; } 
    
    [ForeignKey("LocationId")] 
    public int? LocationId { get; set; }

    
    public Location? Location { get; set; }
    
    
    public string? BackgroundPhoto { get; set; }
    public string? ProfilePhoto { get; set; }
    
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }
    
    public List<Gallery>? Galleries{ get; set; } 

    public User()
    {
        Images = new List<Image>();
        Galleries = new List<Gallery>();
    }

  

}