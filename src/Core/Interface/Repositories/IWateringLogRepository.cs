using Core.Entities;

namespace Core.Interface.Repositories;

public interface IWateringLogRepository : IRepository<WateringLog>
{
    Task<List<WateringLog>> GetWateringLogsByPlantAsync(Guid id, CancellationToken token);
    Task<List<WateringLog>> GetRecentAsync(int count, CancellationToken cancellationToken);
}