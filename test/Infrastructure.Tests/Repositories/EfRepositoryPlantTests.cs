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
}
