namespace Core.Entities;

public class Plant
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Species { get; set; }

    public string Location { get; set; }

    public DateTime? LastWatered { get; set; }

    public string? ImageUrl { get; set; } 
}