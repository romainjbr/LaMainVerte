using Core.Dtos;
using Core.Entities;
using Core.Interface.Services;
using Core.Interface.Repositories;
using Core.Services;
using Moq;
using Xunit;

namespace Core.Tests.Services.WateringLogServiceTest;

public class WateringLogServiceTests
{
    private readonly Mock<IWateringLogRepository> _repo;
    private readonly IWateringLogService _svc;

    private static WateringLog MakeLog(Guid? id = null, Guid? plantId = null) => new()
    {
        Id = id ?? Guid.NewGuid(),
        PlantId = plantId ?? Guid.NewGuid(),
        Date = DateTime.UtcNow.AddDays(-1)
    };

    private static WateringLogCreateDto MakeCreateDto(Guid? plantId = null) =>
        new(PlantId: plantId ?? Guid.NewGuid(), Date: DateTime.UtcNow);

    private static WateringLogUpdateDto MakeUpdateDto(Guid id, Guid? plantId = null) =>
        new(Id: id, PlantId: plantId ?? Guid.NewGuid(), Date: DateTime.UtcNow.AddHours(-5));

    public WateringLogServiceTests()
    {
        _repo = new Mock<IWateringLogRepository>();
        _svc = new WateringLogService(_repo.Object);
    }

    #region AddAsync

    [Fact]
    public async Task AddAsync_CallsRepositoryWithMappedEntity()
    {
        var dto = MakeCreateDto();

        _repo.Setup(x => x.AddAsync(It.IsAny<WateringLog>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        await _svc.AddAsync(dto, CancellationToken.None);

        _repo.Verify(x => x.AddAsync(
            It.Is<WateringLog>(w => w.PlantId == dto.PlantId &&w.Date == dto.Date), 
                It.IsAny<CancellationToken>()), Times.Once);
    }

    #endregion

    #region DeleteAsync

    [Fact]
    public async Task DeleteAsync_NotFound_ReturnsFalse()
    {
        _repo.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((WateringLog?)null);

        var ok = await _svc.DeleteAsync(Guid.NewGuid(), CancellationToken.None);

        Assert.False(ok);
        _repo.Verify(x => x.DeleteAsync(It.IsAny<WateringLog>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_Found_DeletesAndReturnsTrue()
    {
        var existing = MakeLog();

        _repo.Setup(x => x.GetByIdAsync(existing.Id, It.IsAny<CancellationToken>())).ReturnsAsync(existing);

        _repo.Setup(x => x.DeleteAsync(existing, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        var ok = await _svc.DeleteAsync(existing.Id, CancellationToken.None);

        Assert.True(ok);
        _repo.Verify(x => x.DeleteAsync(existing, It.IsAny<CancellationToken>()), Times.Once);
    }

    #endregion

        #region GetRecentAsync

    [Fact]
    public async Task GetRecentAsync_ReturnsOrderedDtoList()
    {
        var log1 = MakeLog(id: Guid.NewGuid());
        log1.Date = DateTime.UtcNow.AddDays(-2);

        var log2 = MakeLog(id: Guid.NewGuid());
        log2.Date = DateTime.UtcNow.AddDays(-1);

        var logs = new List<WateringLog> { log1, log2 };

        _repo.Setup(x => x.GetRecentAsync(2, It.IsAny<CancellationToken>()))
            .ReturnsAsync(logs);

        var result = await _svc.GetRecentAsync(2, CancellationToken.None);

        Assert.Equal(2, result.Count);
        Assert.True(result[0].Date >= result[1].Date, "the Results must be sorted descending by date");
        Assert.Contains(log1.Id, result.Select(r => r.Id));
        Assert.Contains(log2.Id, result.Select(r => r.Id));
    }

    #endregion

    #region GetWateringLogsByPlantAsync

    [Fact]
    public async Task GetWateringLogsByPlantAsync_ReturnsOrderedDtoList()
    {
        var plantId = Guid.NewGuid();

        var log1 = MakeLog(plantId: plantId);
        log1.Date = DateTime.UtcNow.AddHours(-3);

        var log2 = MakeLog(plantId: plantId);
        log2.Date = DateTime.UtcNow.AddHours(-1);

        var logs = new List<WateringLog> { log1, log2 };

        _repo.Setup(x => x.GetWateringLogsByPlantAsync(plantId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(logs);

        var result = await _svc.GetWateringLogsByPlantAsync(plantId, CancellationToken.None);

        Assert.Equal(2, result.Count);
        Assert.True(result[0].Date >= result[1].Date);
        Assert.All(result, r => Assert.Equal(plantId, r.PlantId));
    }

    #endregion


    #region UpdateAsync

    [Fact]
    public async Task UpdateAsync_ThrowsNotImplementedException()
    {
        var dto = MakeUpdateDto(Guid.NewGuid());

        await Assert.ThrowsAsync<NotImplementedException>(() =>
            _svc.UpdateAsync(dto, CancellationToken.None));
    }

    #endregion
}