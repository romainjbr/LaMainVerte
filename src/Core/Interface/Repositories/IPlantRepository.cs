using Core.Entities;

namespace Core.Interface.Repositories;

public interface IPlantRepository : IRepository<Plant>
{
    Task<List<Plant>> GetPlantsNeedingWater(CancellationToken token);
}