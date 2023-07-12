using System.ComponentModel.DataAnnotations.Schema;

namespace BackendLi.Entities;

[Table("location")]
public class Location
{
    public int Id { get; set; }
    public string? Country { get; set; }
    public string? City { get; set; }
}