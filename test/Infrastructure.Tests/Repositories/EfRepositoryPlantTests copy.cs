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
    public async Task ListAsync_Returns_All_Plants()
    {
        var firstPlant = new Plant
        {
            Id = Guid.NewGuid(),
            Name = "Pothos",
            Species = "Epipremnum aureum",
            Location = "Kitchen",
            WaterFrequency = WaterFrequency.Weekly
        };

        var secondPlant = new Plant
        {
            Id = Guid.NewGuid(),
            Name = "Blue plant",
            Species = "Dracaena trifasciata",
            Location = "Bedroom",
            WaterFrequency = WaterFrequency.Monthly
        };

        await _repo.AddAsync(firstPlant, CancellationToken.None);
        await _repo.AddAsync(secondPlant, CancellationToken.None);

        var list = await _repo.ListAsync(CancellationToken.None);

        Assert.Equal(2, list.Count);
        Assert.Contains(list, p => p.Id == firstPlant.Id);
        Assert.Contains(list, p => p.Id == secondPlant.Id);
    }

    [Fact]
    public async Task UpdateAsync_Persists_Changes()
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
        plant.ImageUrl = "https://example.com/fern.png";

        await _repo.UpdateAsync(plant, CancellationToken.None);

        var reloaded = await _repo.GetByIdAsync(plant.Id, CancellationToken.None);

        Assert.NotNull(reloaded);
        Assert.Equal("Bathroom", reloaded!.Location);
        Assert.Equal(WaterFrequency.Daily, reloaded.WaterFrequency);
        Assert.Equal("https://example.com/fern.png", reloaded.ImageUrl);
    }

    [Fact]
    public async Task DeleteAsync_Removes_Plant()
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

    [Fact]
    public async Task GetByIdAsync_Returns_Null_When_NotFound()
    {
        var result = await _repo.GetByIdAsync(Guid.NewGuid(), CancellationToken.None);

        Assert.Null(result);
    }
}
