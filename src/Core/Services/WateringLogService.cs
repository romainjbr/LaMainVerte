using Core.Dtos;
using Core.Interface.Services;

namespace Core.Services;

public class WateringLogService : IWateringLogService
{
    private readonly IWateringLogRepository _repo; 

    public WateringLogService(IWateringLogRepository repo)
    {
        _repo = repo;
    }

    public async Task AddAsync(WateringLogCreateDto dto, CancellationToken token)
    {
        await _repo.AddAsync(dto.ToEntity(), token);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken token)
    {
        var entity = await _repo.GetByIdAsync(id, token);
        if (entity is null) { return false; }

        await _repo.DeleteAsync(entity, token);
        return true;
    }

    public async Task<List<WateringLogReadDto>> GetRecentAsync(int count, CancellationToken token)
    {
        var logs = await _repo.GetRecentAsync(count, token);
        
        return logs
        .Select(w => w.ToDto())
        .OrderByDescending(w => w.Date)
        .ToList();
    }

    public async Task<List<WateringLogReadDto>> GetWateringLogsByPlantAsync(Guid plantId, CancellationToken token)
    {
        var logs = await _repo.GetWateringLogsByPlantAsync(plantId, token);

        return logs
        .Select(w => w.ToDto())
        .OrderByDescending(w => w.Date)
        .ToList();
    }

    public Task<bool> UpdateAsync(WateringLogUpdateDto dto, CancellationToken token)
    {
        throw new NotImplementedException();
    }
}