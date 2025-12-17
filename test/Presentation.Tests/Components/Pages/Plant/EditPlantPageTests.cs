using Bunit;
using Core.Dtos;
using Core.Enums;
using Core.Interface.Services;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Presentation.Components.Forms;
using Presentation.Components.Pages.Plant;

namespace Presentation.Tests.Components.Pages;

public class EditPlantPageTests : BunitContext
{
    private readonly Mock<IPlantService> _svc;
    private readonly Mock<IPlantImageService> _imgSvc;

    public EditPlantPageTests()
    {
        _svc = new Mock<IPlantService>();
        _imgSvc = new Mock<IPlantImageService>();
    }

    [Fact]
    public void CallsPage_InitiallyDisplaysLoading()
    {
        var tcs = new TaskCompletionSource<PlantReadDto?>(); 
            
        _svc.Setup(s => s.GetPlantByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .Returns(tcs.Task);

        Services.AddSingleton(_svc.Object);
        Services.AddSingleton(_imgSvc.Object);

        var page = Render<EditPlant>(parameters => parameters.Add(p => p.Id, Guid.NewGuid()));

        Assert.Contains("Loading...", page.Markup);
    }

    [Fact]
    public void CallsPage_WithPlantID_DisplaysPlantName()
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

        var tcs = new TaskCompletionSource<PlantReadDto?>();

        _svc.Setup(s => s.GetPlantByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(plantDto);

        Services.AddSingleton(_svc.Object);
        Services.AddSingleton(_imgSvc.Object);

        var page = Render<EditPlant>(parameters => parameters.Add(p => p.Id, plantId));

        Assert.Contains("Monstera", page.Markup);
    }

}
