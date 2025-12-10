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

    public bool NeedsWater()
    {
        return GetWateredStatus() == WaterStatus.OVERDUE || GetWateredStatus() == WaterStatus.NEVER_WATERED;
    }

    public int GetDaysSinceLastWatered()
    {
        if (LastWatered is null) { return int.MaxValue; }

        return (int)(DateTime.Today.Date - LastWatered.Value.Date).TotalDays;
    }

    public WaterStatus GetWateredStatus()
    {
        if (LastWatered is null) 
        {
            return WaterStatus.NEVER_WATERED;
        }

        var daysSince = (DateTime.Today - LastWatered.Value.Date).TotalDays;
        var freqDays = WaterFrequency.GetFrequencyDays();

        if (daysSince <= freqDays * 0.5)
        {
            return WaterStatus.WELL_WATERED;
        }

        if (daysSince <= freqDays)
        {
            return WaterStatus.DUE_SOON;
        }

        return WaterStatus.OVERDUE;
    }
}