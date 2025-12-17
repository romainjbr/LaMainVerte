using Core.Entities;
using Core.Interface.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class PlantRepository : EfRepository<Plant>, IPlantRepository
{
    public PlantRepository(PlantDbContext db ) : base(db) {}

    public async Task<List<Plant>> GetPlantsNeedingWater(CancellationToken token)
    {
        var allPlants = await _db.Set<Plant>()
            .AsNoTracking()
            .ToListAsync(token);

        var plantsNeedingWater = allPlants
            .Where(p => p.NeedsWater())
            .OrderByDescending(p => p.GetDaysSinceLastWatered())
            .ToList();

        return plantsNeedingWater ?? [];
    }
}
