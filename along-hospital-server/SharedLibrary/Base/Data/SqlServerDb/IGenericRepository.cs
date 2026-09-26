using SharedLibrary.Commons.EntityAbstractions;
using System.Linq.Expressions;

namespace SharedLibrary.Base.Data.SqlServerDb
{
    public interface IGenericRepository<T> where T : Entity
    {
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, string[]? includes = null, CancellationToken cancellationToken = default);
        IQueryable<T> GetAllQueryable(string[]? includes = null);
        Task<List<T>> GetAllByIdsAsync(List<int> ids, string[]? includes = null, CancellationToken cancellationToken = default);

        Task<(int Count, List<T> Items)> GetAllPaginatedAsync(string? filter, string order, int page, int pageSize, string[]? includes = null, CancellationToken cancellationToken = default);
        Task<(int Count, List<T> Items)> GetAllPaginatedAsync(Expression<Func<T, bool>>? filter, string order, int page, int pageSize, string[]? includes = null, CancellationToken cancellationToken = default);
        Task<(int Count, List<T> Items)> GetAllPaginatedAsync(Expression<Func<T, bool>>? filterExpr, string? filterStr, string order, int page, int pageSize, string[]? includes = null, CancellationToken cancellationToken = default);

        Task<T?> GetByIdAsync(int id, string[]? includes = null, CancellationToken cancellationToken = default);
        Task<T?> GetByConditionAsync(Expression<Func<T, bool>> filter, string[]? includes = null, CancellationToken cancellationToken = default);

        Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);
        Task AddRangeAsync(List<T> entities, CancellationToken cancellationToken = default);

        void Remove(T entity);
        void RemoveRange(List<T> entities);

        T Update(T entity);
        void UpdateRange(List<T> entities);

        Task<bool> AnyAsync(Expression<Func<T, bool>>? filter = null, CancellationToken cancellationToken = default);
        Task<int> CountAsync(Expression<Func<T, bool>>? filter = null, CancellationToken cancellationToken = default);
    }
}