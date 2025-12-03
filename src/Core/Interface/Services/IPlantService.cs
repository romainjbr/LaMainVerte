using Core.Dtos;

namespace Core.Interface.Services;

public interface IPlantService
{
    Task AddPlantAsync(PlantCreateDto dto, CancellationToken token);
    Task<bool> UpdatePlantAsync(PlantUpdateDto dto, CancellationToken token);
    Task<bool> DeletePlantAsync(Guid id, CancellationToken token);
    Task<List<PlantReadDto>> GetAllPlantsAsync(CancellationToken token);
    Task<PlantReadDto?> GetPlantByIdAsync(Guid id, CancellationToken token);
    Task<bool> WaterPlant(Guid id, CancellationToken token);
}