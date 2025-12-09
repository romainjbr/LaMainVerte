using Core.Entities;
using Core.Enums;

namespace Core.Dtos;

public static class PlantMapper
{ 
    public static PlantReadDto ToDto(this Plant plant)
    {
        return new PlantReadDto(
            plant.Id, 
            plant.Name, 
            plant.Species,
            plant.Location, 
            plant.LastWatered, 
            plant.WaterFrequency,
            plant.ImageUrl,
            plant.WateringLogs.Select(x => x.ToDto()).ToList(),
            plant.GetWateredStatus());
    }

    public static Plant ToEntity(this PlantCreateDto dto)
    {
        return new Plant
        {
            Id = new Guid(), 
            Name = dto.Name, 
            Species = dto.Species, 
            Location = dto.Location, 
            LastWatered = dto.LastWatered, 
            WaterFrequency = dto.WaterFrequency, 
            ImageUrl = dto.ImageUrl
        };
    }

    public static Plant ToEntity(this PlantReadDto dto)
    {
        return new Plant
        {
            Id = new Guid(), 
            Name = dto.Name, 
            Species = dto.Species, 
            Location = dto.Location, 
            LastWatered = dto.LastWatered, 
            WaterFrequency = dto.WaterFrequency, 
            ImageUrl = dto.ImageUrl
        };
    }
}