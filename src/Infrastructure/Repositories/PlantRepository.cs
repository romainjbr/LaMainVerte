using Core.Entities;
using Core.Interface.Repositories;
using Infrastructure.Data;

namespace Core.Interfaces.Repositories;

public class PlantRepository : EfRepository<Plant>, IPlantRepository
{
    public PlantRepository(PlantDbContext db ) : base(db) {}

    public async Task<List<Plant>> GetPlantsNeedingWater(CancellationToken token)
    {
        return new List<Plant>();
    }
}
