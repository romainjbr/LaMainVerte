using Bunit;
using Core.Dtos;
using Core.Enums;
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

    [Fact]
    public void CallsPage_Found_DisplayPlantProperties()
    {
        var plantId = Guid.NewGuid();

        var plantDto = new PlantReadDto
        (
            plantId,
            "Monstera",
            "Monstera deliciosa",
            "Living Room",
            DateTime.UtcNow,
            WaterFrequency.Weekly,
            "monstera.png",
            default,
            default
        );
        
        _plantSvc.Setup(s => s.GetPlantByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(plantDto);

        Services.AddSingleton(_plantSvc.Object);
        Services.AddSingleton(_WateringLogsvc.Object);

        var page = Render<PlantDetails>(parameters => parameters.Add(p => p.Id, plantId));

        Assert.Contains("Monstera", page.Markup);
        Assert.Contains("Monstera deliciosa", page.Markup);
        Assert.Contains("Living Room", page.Markup);
        Assert.Contains("monstera.png", page.Markup);
    }
}