using Core.Enums;

namespace Core.Entities;

public class Plant
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Species { get; set; } = string.Empty;

    public string Location { get; set; } = string.Empty;

    public DateTime? LastWatered { get; set; }

    public WaterFrequency WaterFrequency { get; set; }

    public string? ImageUrl { get; set; } 

    public List<WateringLog> WateringLogs { get; set; } = new();

    public void Water()
    {
        LastWatered = DateTime.UtcNow;
    }
}