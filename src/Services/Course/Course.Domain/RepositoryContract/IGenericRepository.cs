using System.Linq.Expressions;

namespace Course.Domain.RepositoryContract
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, string includeProperties = "", string? sortBy = null, string? sortDirection = "asc", int? pageIndex = null, int? pageSize = null);
        Task<T> GetByAsync(Expression<Func<T, bool>>? filter = null, bool isTracked = true, string includeProperties = "");
        Task<T> CreateAsync(T model);
        Task<long> CountAsync(Expression<Func<T, bool>>? filter = null);
        Task<bool> AnyAsync(Expression<Func<T, bool>> filter);
        Task AddRangeAsync(IEnumerable<T> model);
        Task RemoveRangeAsync(IEnumerable<T> model);
        Task<bool> DeleteAsync(T model);
        Task<T> UpdateAsync(T model);
        void Detach(T entity);
        Task SaveAsync();
        Task<T> CheckEntityAsync(Expression<Func<T, bool>> predicate, string entityName);

    }
}
