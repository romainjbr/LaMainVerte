using Core.Dtos;
using Core.Entities;
using Core.Interface.Services;
using Core.Interface.Repositories;

namespace Core.Services;

public class PlantService : IPlantService
{
    IRepository<Plant> _repo; 

    public PlantService(IRepository<Plant> repo)
    {
        _repo = repo;
    }

    public async Task<bool> WaterPlant(Guid id, CancellationToken token)
    {
        var entity = await _repo.GetByIdAsync(id, token);
        if (entity is null) { return false; }

        entity.Water();

        return true;
    }

    public async Task AddPlantAsync(PlantCreateDto dto, CancellationToken token)
    {
        await _repo.AddAsync(dto.ToEntity(), token);
    }

    public async Task<bool> DeletePlantAsync(Guid id, CancellationToken token)
    {
        var entity = await _repo.GetByIdAsync(id, token);

        if (entity is null) { return false; }

        await _repo.DeleteAsync(entity, token);
        return true;
    }

    public async Task<List<PlantReadDto>> GetAllPlantsAsync(CancellationToken token)
    {
        var all = await _repo.ListAsync(token);
        return all.Select(x => x.ToDto()).ToList();
    }

    public async Task<PlantReadDto?> GetPlantByIdAsync(Guid id, CancellationToken token)
    {
        var entity = await _repo.GetByIdAsync(id, token);
        return entity?.ToDto();
    }

    public async Task<bool> UpdatePlantAsync(PlantUpdateDto dto, CancellationToken token)
    {
        var entity = await _repo.GetByIdAsync(dto.Id, token);

        if (entity is null) { return false; }

        entity.Name = dto.Name;
        entity.Species = dto.Species;
        entity.ImageUrl = dto.ImageUrl;
        entity.LastWatered = dto.LastWatered;
        entity.Location = dto.Location;
        entity.WaterFrequency = dto.WaterFrequency;

        await _repo.UpdateAsync(entity, token);

        return true;
    }
}