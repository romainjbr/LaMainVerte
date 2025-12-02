using Core.Dtos;
using Core.Entities;

namespace Core.Dtos;

public static class WateringLogMapper
{
    public static WateringLogReadDto ToDto(this WateringLog entity)
    {
        return new WateringLogReadDto(entity.Id, entity.PlantId, entity.Date);
    }

    public static WateringLog ToEntity(this WateringLogCreateDto dto)
    {
        return new WateringLog
        {
            Id = Guid.NewGuid(),
            PlantId = dto.PlantId,
            Date = dto.Date,
        };
    }
}