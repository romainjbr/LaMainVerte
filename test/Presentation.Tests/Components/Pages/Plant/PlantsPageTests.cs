using Bunit;
using Core.Dtos;
using Core.Interface.Services;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Presentation.Components.Pages.Plant;

namespace Presentation.Tests.Components.Pages;

public class PlantsPageTests : BunitContext
{
    private readonly Mock<IPlantService> _svc;
    private readonly Mock<IPlantImageService> _imgSvc;

    public PlantsPageTests()
    {
        _svc = new Mock<IPlantService>();
        _imgSvc = new Mock<IPlantImageService>();
    }

    [Fact]
    public void CallsPage_InitiallyDisplaysLoading()
    {
        Services.AddSingleton(_svc.Object);
        Services.AddSingleton(_imgSvc.Object);

        var page = Render<Plants>();

        Assert.Contains("Loading plants...", page.Markup);
    }
}
