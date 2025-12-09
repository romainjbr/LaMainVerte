using Core.Enums;

namespace Core.Dtos;

public record PlantReadDto(Guid Id, string Name, string Species, string Location, DateTime? LastWatered, WaterFrequency WaterFrequency, string? ImageUrl, List<WateringLogReadDto>? WateringLogs, WaterStatus? WaterStatus);