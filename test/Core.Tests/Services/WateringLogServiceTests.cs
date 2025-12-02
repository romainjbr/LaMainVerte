using Core.Dtos;
using Core.Entities;
using Core.Interface.Services;
using Core.Interface.Repositories;
using Core.Services;
using Moq;

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
}