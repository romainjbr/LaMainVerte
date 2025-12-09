using Core.Entities;
using Core.Interface.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Core.Interfaces.Repositories;

public class PlantRepository : EfRepository<Plant>, IPlantRepository
{
    public PlantRepository(PlantDbContext db ) : base(db) {}

    public async Task<List<Plant>> GetPlantsNeedingWater(CancellationToken token)
    {
        var plantsNeedingWater = await _db.Set<Plant>()
            .AsNoTracking()
            .Where(p => p.NeedsWater())
            .OrderByDescending(p => p.GetDaysSinceLastWatered())
            .ToListAsync();

        return plantsNeedingWater;
    }
}
