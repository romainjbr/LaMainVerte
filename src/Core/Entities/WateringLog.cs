namespace Core.Entities;

public class WateringLog
{
    public Guid Id { get; set; }

    public Guid PlantId { get; set; }
    public Plant Plant { get; set; } = null!;

    public DateTime Date { get; set; } = DateTime.Now;
}
