using Core.Entities;
using Core.Enums;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Tests.Repositories;

public class EfRepositoryPlantTests
{
    public EfRepository<Plant> _repo;

    public EfRepositoryPlantTests()
    {
        var options = new DbContextOptionsBuilder<PlantDbContext>().UseSqlite("Filename=:memory:").Options;

        var db = new PlantDbContext(options);
        db.Database.OpenConnection();
        db.Database.EnsureCreated();      

        _repo = new EfRepository<Plant>(db);
    }
    
    #region  GetIdByAsync

    [Fact]
    public async Task AddAndGetById_Success_ReturnsCorrectPlant()
    {
        var plant = new Plant
        {
            Id = Guid.NewGuid(),
            Name = "Monstera",
            Species = "Monstera deliciosa",
            Location = "Living room",
            WaterFrequency = WaterFrequency.Weekly,
            ImageUrl = "https://example.com/monstera.jpg"
        };

        await _repo.AddAsync(plant, CancellationToken.None);

        var result = await _repo.GetByIdAsync(plant.Id, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal("Monstera", result!.Name);
        Assert.Equal("Monstera deliciosa", result.Species);
        Assert.Equal("Living room", result.Location);
        Assert.Equal(WaterFrequency.Weekly, result.WaterFrequency);
        Assert.Equal("https://example.com/monstera.jpg", result.ImageUrl);
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
    public async Task DeleteAsync_PlantFound_RemovePlant()
    {
        var plant = new Plant
        {
            Id = Guid.NewGuid(),
            Name = "Aloe",
            Species = "Aloe vera",
            Location = "Bedroom",
            WaterFrequency = WaterFrequency.Weekly
        };

        await _repo.AddAsync(plant, CancellationToken.None);

        Assert.NotNull(await _repo.GetByIdAsync(plant.Id, CancellationToken.None));

        await _repo.DeleteAsync(plant, CancellationToken.None);

        var result = await _repo.GetByIdAsync(plant.Id, CancellationToken.None);
        Assert.Null(result);
    }

    #endregion

    #region UpdateAsync
    [Fact]
    public async Task UpdateAsync_Success_PersistsChanges()
    {
        var plant = new Plant
        {
            Id = Guid.NewGuid(),
            Name = "Plantix",
            Species = "Nephrolepis exaltata",
            Location = "Hallway",
            WaterFrequency = WaterFrequency.Weekly
        };

        await _repo.AddAsync(plant, CancellationToken.None);

        plant.Location = "Bathroom";
        plant.WaterFrequency = WaterFrequency.Daily;
        plant.ImageUrl = "https://example.com/plantix.png";

        await _repo.UpdateAsync(plant, CancellationToken.None);

        var reloaded = await _repo.GetByIdAsync(plant.Id, CancellationToken.None);

        Assert.NotNull(reloaded);
        Assert.Equal("Bathroom", reloaded!.Location);
        Assert.Equal(WaterFrequency.Daily, reloaded.WaterFrequency);
        Assert.Equal("https://example.com/plantix.png", reloaded.ImageUrl);
    }  

    #endregion
}
