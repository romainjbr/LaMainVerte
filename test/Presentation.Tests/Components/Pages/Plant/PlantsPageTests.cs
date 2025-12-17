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

    public PlantsPageTests()
    {
        _svc = new Mock<IPlantService>();
    }

    [Fact]
    public void CallsPage_InitiallyDisplaysLoading()
    {
        Services.AddSingleton(_svc.Object);

        var page = Render<Plants>();

        Assert.Contains("Loading plants...", page.Markup);
    }
}
