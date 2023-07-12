using BackendLi.Entities;

namespace BackendLi.DTOs;

public class UserDto
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Name { get; set; }
    public Location? Location { get; set; }
    public string Description { get; set; }

    public string BackgroundPhoto { get; set; }
    public string ProfilePhoto { get; set; }
}