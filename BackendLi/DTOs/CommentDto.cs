namespace BackendLi.DTOs;

public class CommentDto
{
    public int Id { get; set; }
public int userId { get; set; }
public string CommentText { get; set; }
public DateTime CreatedAt { get; set; }
}