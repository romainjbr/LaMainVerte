using Bunit;
using Core.Dtos;
using Core.Interface.Services;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Presentation.Components.Pages.Plant;

namespace Presentation.Tests.Components.Pages;

public class EditPlantPageTests : BunitContext
{
    private readonly Mock<IPlantService> _svc;

    public EditPlantPageTests()
    {
        _svc = new Mock<IPlantService>();
    }

    [Fact]
    public void CallsPage_InitiallyDisplaysLoading()
    {
        _svc.Setup(s => s.GetPlantByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((PlantReadDto?)null);

        Services.AddSingleton(_svc.Object);

        var page = Render<EditPlant>(parameters => parameters.Add(p => p.Id, Guid.NewGuid()));

        page.Markup.Contains("Loading...");
    }
}
