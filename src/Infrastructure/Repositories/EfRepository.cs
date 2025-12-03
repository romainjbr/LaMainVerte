
using System.Linq.Expressions;
using Core.Interface.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Core.Interfaces.Repositories;

public class EfRepository<T> : IRepository<T> where T : class
{
    protected readonly PlantDbContext _db;

    public EfRepository(PlantDbContext db) 
    {
        _db = db;     
    }

    public async Task AddAsync(T entity, CancellationToken token)
    {
        _db.Set<T>().Add(entity);
        await _db.SaveChangesAsync(token);
    }

    public async Task DeleteAsync(T entity, CancellationToken token)
    {
        _db.Set<T>().Remove(entity);
        await _db.SaveChangesAsync(token);
    }

    public async Task<T?> GetByIdAsync(Guid id, CancellationToken token)
    {
        return await _db.Set<T>().FindAsync(id, token);
    }

    public async Task<List<T>> ListAsync(CancellationToken token)
    {
        return await _db.Set<T>().ToListAsync(token);
    }

    public async Task UpdateAsync(T entity, CancellationToken token)
    {
        _db.Set<T>().Update(entity);
        await _db.SaveChangesAsync(token);
    }
}