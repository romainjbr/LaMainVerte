using Bunit;
using Core.Dtos;
using Core.Interface.Services;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Presentation.Components.Pages.Plant;

namespace Presentation.Tests.Components.Pages;

public class PlantDetailsPageTests : BunitContext
{
    private readonly Mock<IPlantService> _plantSvc;
    private readonly Mock<IWateringLogService> _WateringLogsvc;

    public PlantDetailsPageTests()
    {
        _plantSvc = new Mock<IPlantService>();
        _WateringLogsvc = new Mock<IWateringLogService>();
    }

    [Fact]
    public void CallsPage_NotFound_DisplayMessage()
    {
        _plantSvc.Setup(s => s.GetPlantByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((PlantReadDto?)null);

        Services.AddSingleton(_plantSvc.Object);
        Services.AddSingleton(_WateringLogsvc.Object);

        var page = Render<PlantDetails>(parameters => parameters.Add(p => p.Id, Guid.NewGuid()));

        Assert.Contains("Plant not found!", page.Markup);
    }
}