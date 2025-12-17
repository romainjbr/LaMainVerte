using Core.Entities;
using Core.Enums;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Tests.Repositories;

public class EfRepositoryWateringLogTests
{
    public EfRepository<WateringLog> _repo;

    public EfRepositoryWateringLogTests()
    {
        var options = new DbContextOptionsBuilder<PlantDbContext>().UseSqlite("Filename=:memory:").Options;

        var db = new PlantDbContext(options);
        db.Database.OpenConnection();
        db.Database.EnsureCreated();      

        _repo = new EfRepository<WateringLog>(db);
    }

    public static Plant GetPlant(Guid? id) => new Plant
    {
        Id = id ?? Guid.NewGuid(),
        Name = "Monstera",
        Species = "Monstera deliciosa",
        Location = "Living room",
        WaterFrequency = WaterFrequency.Weekly,
        ImageUrl = "https://example.com/monstera.jpg"
    };

    public static WateringLog GetWateringLog(Guid? id, Guid plantId, Plant plant) => new WateringLog
    {
        Id = id ?? Guid.NewGuid(),
        PlantId = plantId,
        Plant = plant,
        Date = DateTime.UtcNow       
    };
    
    #region  GetIdByAsync

    [Fact]
    public async Task AddAndGetById_Success_ReturnsCorrectPlant()
    {
        var plantId = Guid.NewGuid();
        var wateringLogId = Guid.NewGuid();

        var plant = GetPlant(plantId);
        var wateringLog = GetWateringLog(wateringLogId, plantId, plant);

        await _repo.AddAsync(wateringLog, CancellationToken.None);

        var result = await _repo.GetByIdAsync(wateringLog.Id, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(wateringLogId, result.Id);
        Assert.Equal(plantId, result.PlantId);
    } 

    [Fact]
    public async Task GetByIdAsync_NotFound_ReturnsNull()
    {
        var result = await _repo.GetByIdAsync(Guid.NewGuid(), CancellationToken.None);

        Assert.Null(result);
    }

    #endregion

    #region  DeleteAsync

    [Fact]
    public async Task DeleteAsync_WateringLogFound_RemoveWateringLog()
    {
        var plantId = Guid.NewGuid();
        var wateringLogId = Guid.NewGuid();

        var plant = GetPlant(plantId);
        var wateringLog = GetWateringLog(wateringLogId, plantId, plant);

        await _repo.AddAsync(wateringLog, CancellationToken.None);

        Assert.NotNull(await _repo.GetByIdAsync(wateringLog.Id, CancellationToken.None));

        await _repo.DeleteAsync(wateringLog, CancellationToken.None);

        var result = await _repo.GetByIdAsync(wateringLog.Id, CancellationToken.None);
        Assert.Null(result);
    }

    #endregion
}
