namespace BackendLi.DTOs;

public class SuccessResponseDto
{
    public SuccessResponseDto(string message)
    {
        Message = message;
    }

    public string Message { get; set; }
}