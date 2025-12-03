using Core.Dtos;
using Core.Enums;

public class PlantFormModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Species { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public DateTime? LastWatered { get; set; }
    public WaterFrequency? WaterFrequency { get; set; }
    public List<WateringLogReadDto> WateringLogs { get; set; } = new();
    public string ImageUrl { get; set; } = string.Empty;
}
