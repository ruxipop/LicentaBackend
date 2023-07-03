namespace BackendLi.DTOs;

public class StatisticsTypeDto
{
    public StatisticsTypeDto(string type, int number)
    {
        Type = type;
        Number = number;
    }

    public string Type { get; set; }
    public int Number { get; set; }
}