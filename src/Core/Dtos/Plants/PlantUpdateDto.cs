using Core.Entities;
using Core.Enums;

namespace Core.Dtos;

public record PlantUpdateDto(Guid Id, string Name, string Species, string Location, DateTime LastWatered, WaterFrequency WaterFrequency, List<WateringLogReadDto>? WateringLogs, string? ImageUrl);