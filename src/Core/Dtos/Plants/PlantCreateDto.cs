using Core.Enums;

namespace Core.Dtos;

public record PlantCreateDto(string Name, string Species, string Location, DateTime? LastWatered, WaterFrequency WaterFrequency, string? ImageUrl);