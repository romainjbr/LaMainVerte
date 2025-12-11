
namespace Core.Interface.Services;

public interface IPlantImageService
{
    Task<string> SavePlantImageAsync(Stream content, string fileName, string contentType, CancellationToken cancellationToken);            
}