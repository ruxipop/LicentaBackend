using System.ComponentModel.DataAnnotations.Schema;

namespace BackendLi.Entities;

[Table("report")]
public class Report
{

    public int Id { get; set; }

    public string? Name { get; set; }
    public string? Email { get; set; }
    public string Message { get; set; }
    
    [ForeignKey("ImageId")] public int? ImageId { get; set; }

    public virtual Photo? Image { get; set; }
    
    [ForeignKey("UserId")] public int? UserId { get; set; }

    public virtual User? User { get; set; }
    protected Report() { }

    public static Report Create() => new Report();

    public Report WithName(string name)
    {
        Name = name;
        return this;
    }

    public Report WithEmail(string email)
    {
        Email = email;
        return this;
    }

    public Report WithMessage(string message)
    {
        Message = message;
        return this;
    }

    public Report WithImageId(int? imageId)
    {
        ImageId = imageId;
        return this;
    }

    public Report WithImage(Photo? image)
    {
        Image = image;
        return this;
    }

    public Report WithUserId(int? userId)
    {
        UserId = userId;
        return this;
    }

    public Report WithUser(User? user)
    {
        User = user;
        return this;
    }
}