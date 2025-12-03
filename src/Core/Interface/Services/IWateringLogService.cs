using Core.Dtos;

namespace Core.Interface.Services;

public interface IWateringLogService
{
    Task AddAsync(WateringLogCreateDto dto, CancellationToken token);
    Task<bool> UpdateAsync(WateringLogUpdateDto dto, CancellationToken token);
    Task<bool> DeleteAsync(Guid id, CancellationToken token);
    Task<List<WateringLogReadDto>> GetWateringLogsByPlantAsync(Guid id, CancellationToken token);
    Task<List<WateringLogReadDto>> GetRecentAsync(int count, CancellationToken cancellationToken);
}