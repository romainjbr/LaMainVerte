using Bunit;
using Core.Dtos;
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
}
