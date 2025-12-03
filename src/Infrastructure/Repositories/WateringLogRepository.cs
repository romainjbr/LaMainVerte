using Core.Entities;
using Core.Interface.Repositories;
using Core.Interfaces.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

public class WateringLogRepository : EfRepository<WateringLog>, IWateringLogRepository
{
    public WateringLogRepository(PlantDbContext db ) : base(db) {}

    public async Task<List<WateringLog>> GetRecentAsync(int count, CancellationToken token)
    {
        var logs = await _db.Set<WateringLog>()
            .AsNoTracking()
            .OrderByDescending(w => w.Date)
            .Take(10)
            .ToListAsync(token);

        return logs;
    }

    public async Task<List<WateringLog>> GetWateringLogsByPlantAsync(Guid id, CancellationToken token)
    {
        var logs = await _db.Set<WateringLog>()
            .AsNoTracking()
            .Where(w => w.PlantId == id)
            .OrderByDescending(w => w.Date)
            .ToListAsync(token);
        
        return logs;
    }
}