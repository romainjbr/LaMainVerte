using Bunit;
using Core.Dtos;
using Core.Entities;
using Core.Enums;
using Core.Interface.Services;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Presentation.Components.Forms;
using Presentation.Components.Pages.Dashboard;
using Presentation.Components.Pages.Plant;

namespace Presentation.Tests.Components.Pages;

public class DashboardPageTests : BunitContext
{
    private readonly Mock<IPlantService> _plantSvc;
    private readonly Mock<IWateringLogService> _wateringLogSvc;

    public DashboardPageTests()
    {
        _plantSvc = new Mock<IPlantService>();
        _wateringLogSvc = new Mock<IWateringLogService>();      
    }

    [Fact]
    public void CallsPage_PageLoading_DisplaysMessage()
    {
        var tcs = new TaskCompletionSource<List<PlantReadDto>>(); 

        _plantSvc.Setup(s => s.GetAllPlantsAsync(It.IsAny<CancellationToken>())).Returns(tcs.Task);

        Services.AddSingleton(_plantSvc.Object);
        Services.AddSingleton(_wateringLogSvc.Object);

        var page = Render<Dashboard>();

        Assert.Contains("Loading...", page.Markup);
    }

    [Fact]
    public void CallsPage_ReturnsList_DisplaysListCount()
    {
        var plantList = new List<PlantReadDto>
        {
            new (Guid.NewGuid(), "Monsterosa", "Monsterosa deliciosa", "Living Room", DateTime.Now, WaterFrequency.Daily, "", default, default),
            new (Guid.NewGuid(), "Snaky", "Snake plant", "Bedoom", DateTime.Now, WaterFrequency.Weekly, "", default, default), 
        };

        _plantSvc.Setup(s => s.GetAllPlantsAsync(It.IsAny<CancellationToken>())).ReturnsAsync(plantList);
        _plantSvc.Setup(s => s.GetPlantsNeedingWater(It.IsAny<CancellationToken>())).ReturnsAsync(new List<PlantReadDto>());
        _wateringLogSvc.Setup(s => s.GetRecentAsync(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(new List<WateringLogReadDto>());

        Services.AddSingleton(_plantSvc.Object);
        Services.AddSingleton(_wateringLogSvc.Object);
                
        var page = Render<Dashboard>();

        Assert.Contains("Total plants", page.Markup);
        Assert.Contains($"{plantList.Count}", page.Markup);
    }
}
