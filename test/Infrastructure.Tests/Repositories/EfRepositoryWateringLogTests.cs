using Core.Entities;
using Core.Enums;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Tests.Repositories;

public class EfRepositoryWateringLogTests
{
    public WateringLogRepository _repo;

    public EfRepositoryWateringLogTests()
    {
        var options = new DbContextOptionsBuilder<PlantDbContext>().UseSqlite("Filename=:memory:").Options;

        var db = new PlantDbContext(options);
        db.Database.OpenConnection();
        db.Database.EnsureCreated();      

        _repo = new WateringLogRepository(db);
    }

    public static Plant GetPlant(Guid id) => new Plant
    {
        Id = id,
        Name = "Monstera",
        Species = "Monstera deliciosa",
        Location = "Living room",
        WaterFrequency = WaterFrequency.Weekly,
        ImageUrl = "https://example.com/monstera.jpg"
    };

    public static WateringLog GetWateringLog(Guid id, Guid plantId, Plant plant) => new WateringLog
    {
        Id = id,
        PlantId = plantId,
        Plant = plant,
        Date = DateTime.UtcNow       
    };
    
    #region  GetIdByAsync

    [Fact]
    public async Task AddAndGetById_Success_ReturnsCorrectPlant()
    {
        var plant = GetPlant(Guid.NewGuid());
        var wateringLog = GetWateringLog(Guid.NewGuid(), plant.Id, plant);

        await _repo.AddAsync(wateringLog, CancellationToken.None);

        var result = await _repo.GetByIdAsync(wateringLog.Id, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(wateringLog.Id, result.Id);
        Assert.Equal(plant.Id, result.PlantId);
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
    public async Task DeleteAsync_WateringLogFound_Remove()
    {
        var plant = GetPlant(Guid.NewGuid());
        var wateringLog = GetWateringLog(Guid.NewGuid(), plant.Id, plant);

        await _repo.AddAsync(wateringLog, CancellationToken.None);

        Assert.NotNull(await _repo.GetByIdAsync(wateringLog.Id, CancellationToken.None));

        await _repo.DeleteAsync(wateringLog, CancellationToken.None);

        var result = await _repo.GetByIdAsync(wateringLog.Id, CancellationToken.None);
        Assert.Null(result);
    }

    #endregion

    #region  DeleteAsync

    [Fact]
    public async Task GetWateringLogs_Found_ReturnsList()
    {
        var plant = GetPlant(Guid.NewGuid());

        var relatedWateringLog_1 = GetWateringLog(Guid.NewGuid(), plant.Id, plant);   
        var relatedWateringLog_2 = GetWateringLog(Guid.NewGuid(), plant.Id, plant);

        var unrelatedLog = GetWateringLog(Guid.NewGuid(), Guid.NewGuid(), new Plant()); 

        await _repo.AddAsync(relatedWateringLog_1, CancellationToken.None);
        await _repo.AddAsync(relatedWateringLog_2, CancellationToken.None);

        await _repo.AddAsync(unrelatedLog, CancellationToken.None);

        var logs = await _repo.GetWateringLogsByPlantAsync(plant.Id, CancellationToken.None);

        Assert.Equal(2, logs.Count);
    }
    
    #endregion
}
