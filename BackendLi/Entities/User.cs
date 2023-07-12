using System.ComponentModel.DataAnnotations.Schema;

namespace BackendLi.Entities;

[Table("users")]
public class User
{
    public User()
    {
        Images = new List<Photo>();
        Galleries = new List<Gallery>();
    }

    public int Id { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }
    public string Name { get; set; }

    public string Username { get; set; }

    public string? Description { get; set; }
    public List<Photo>? Images { get; set; }

    [ForeignKey("LocationId")] public int? LocationId { get; set; }


    public virtual Location? Location { get; set; }


    public string? BackgroundPhoto { get; set; }
    public string? ProfilePhoto { get; set; }

    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }

    public List<Gallery>? Galleries { get; set; }

    public DateTime RegisterDate { get; set; }
}