using Core.Dtos;
using Core.Entities;
using Core.Enums;
using Core.Interface.Repositories;
using Core.Interface.Services;
using Core.Services;
using Moq;

namespace Core.Tests.Services.PlantServiceTest;

public class PlantServiceTests
{
    private readonly Mock<IRepository<Plant>> _repo;
    private readonly IPlantService _svc;

    private static Plant MakePlant(Guid? id = null) => new()
    {
        Id = id ?? Guid.NewGuid(),
        Name = "Monstera",
        Species = "Monstera deliciosa",
        ImageUrl = "https://plant-example/1.png",
        LastWatered = new DateTime(2025, 1, 1),
        Location = "Living Room",
        WaterFrequency = WaterFrequency.EveryTwoDays
    };

    private static PlantCreateDto MakeCreateDto() =>
        new(Name: "Monstera",
            Species: "Monstera deliciosa",
            ImageUrl: "https://plant-example/1.png",
            LastWatered: new DateTime(2025, 1, 1),
            Location: "Living Room",
            WaterFrequency: WaterFrequency.EveryTwoDays);

    private static PlantUpdateDto MakeUpdateDto(Guid id) =>
        new(Id: id,
            Name: "Boston Fern",
            Species: "Nephrolepis exaltata",
            ImageUrl: "https://plant-example/2.png",
            LastWatered: new DateTime(2025, 2, 1),
            Location: "Kitchen",
            WaterFrequency: WaterFrequency.Daily,
            WateringLogs : null);

    public PlantServiceTests()
    {
        _repo = new Mock<IRepository<Plant>>();
        _svc = new PlantService(_repo.Object);
    }

    #region AddPlantSync

    [Fact]
    public async Task AddPlantAsync_CallsRepoWithMappedEntity()
    {
        var dto = MakeCreateDto();

        _repo.Setup(x => x.AddAsync(It.IsAny<Plant>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        await _svc.AddPlantAsync(dto, CancellationToken.None);

        _repo.Verify(x => x.AddAsync(
            It.Is<Plant>(p =>
                p.Name == dto.Name &&
                p.Species == dto.Species &&
                p.ImageUrl == dto.ImageUrl &&
                p.LastWatered == dto.LastWatered &&
                p.Location == dto.Location &&
                p.WaterFrequency == dto.WaterFrequency),
            It.IsAny<CancellationToken>()),
            Times.Once);
    }

    #endregion

    #region GetPlantByIdAsync

    [Fact]
    public async Task GetPlantByIdAsync_NotFound_ReturnsNull()
    {
        _repo.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Plant?)null);

        var result = await _svc.GetPlantByIdAsync(Guid.NewGuid(), CancellationToken.None);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetPlantByIdAsync_Found_ReturnsDto()
    {
        var existing = MakePlant();

        _repo.Setup(x => x.GetByIdAsync(existing.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existing);

        var result = await _svc.GetPlantByIdAsync(existing.Id, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(existing.Id, result!.Id);
        Assert.Equal(existing.Name, result.Name);
        Assert.Equal(existing.Species, result.Species);

        _repo.Verify(x => x.GetByIdAsync(existing.Id, It.IsAny<CancellationToken>()), Times.Once);
    }

    #endregion

    #region GetAllPlantsAsync

    [Fact]
    public async Task GetAllPlantsAsync_ReturnsMappedList()
    {
        var plants = new List<Plant>
        {
            MakePlant(),
            MakePlant()
        };

        _repo.Setup(x => x.ListAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(plants);

        var result = await _svc.GetAllPlantsAsync(CancellationToken.None);

        Assert.Equal(2, result.Count);
        Assert.True(result.All(dto => plants.Any(p => p.Id == dto.Id)));
    }

    #endregion

    #region DeletePlantAsync

    [Fact]
    public async Task DeletePlantAsync_NotFound_ReturnsFalse_DoesNotCallDelete()
    {
        _repo.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Plant?)null);

        var ok = await _svc.DeletePlantAsync(Guid.NewGuid(), CancellationToken.None);

        Assert.False(ok);
        _repo.Verify(x => x.DeleteAsync(It.IsAny<Plant>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task DeletePlantAsync_Found_ReturnsTrue_AndCallsDelete()
    {
        var existing = MakePlant();

        _repo.Setup(x => x.GetByIdAsync(existing.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existing);
        _repo.Setup(x => x.DeleteAsync(existing, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var ok = await _svc.DeletePlantAsync(existing.Id, CancellationToken.None);

        Assert.True(ok);
        _repo.Verify(x => x.DeleteAsync(existing, It.IsAny<CancellationToken>()), Times.Once);
    }

    #endregion

    #region UpdatePlantAsync

    [Fact]
    public async Task UpdatePlantAsync_NotFound_ReturnsFalse_AndDoesNotCallUpdate()
    {
        _repo.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Plant?)null);

        var dto = MakeUpdateDto(Guid.NewGuid());

        var ok = await _svc.UpdatePlantAsync(dto, CancellationToken.None);

        Assert.False(ok);
        _repo.Verify(x => x.UpdateAsync(It.IsAny<Plant>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task UpdatePlantAsync_Found_UpdatesFields_AndCallsUpdate()
    {
        var existingId = Guid.NewGuid();
        var existing = MakePlant(existingId);
        var dto = MakeUpdateDto(existingId);

        _repo.Setup(x => x.GetByIdAsync(existingId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existing);
        _repo.Setup(x => x.UpdateAsync(existing, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var ok = await _svc.UpdatePlantAsync(dto, CancellationToken.None);

        Assert.True(ok);

        Assert.Equal(dto.Name, existing.Name);
        Assert.Equal(dto.Species, existing.Species);
        Assert.Equal(dto.ImageUrl, existing.ImageUrl);
        Assert.Equal(dto.LastWatered, existing.LastWatered);
        Assert.Equal(dto.Location, existing.Location);
        Assert.Equal(dto.WaterFrequency, existing.WaterFrequency);

        _repo.Verify(x => x.UpdateAsync(existing, It.IsAny<CancellationToken>()), Times.Once);
    }

    #endregion
}
