namespace Core.Interface.Repositories;

public interface IRepository<T> where T : class 
{
    Task<List<T>> ListAsync(CancellationToken token);
    Task<T?> GetByIdAsync(Guid id, CancellationToken token);
    Task UpdateAsync(T entity, CancellationToken token); 
    Task DeleteAsync(T entity, CancellationToken token);
    Task AddAsync(T entity, CancellationToken token);
}