namespace BackendLi.DTOs;

public class SuccessResponseDto
{
    public string Message { get; set; }

    public SuccessResponseDto(string message)
    {
        Message = message;
    }
}