using SharedLibrary.Commons.Filters;
using System.Linq.Expressions;

namespace SharedLibrary.Base.Data.MongoDb
{
    public interface IMongoGenericRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null);
        Task<(int Count, List<T> Items)> GetAllPaginatedAsync(MongoFilterDTO filterDTO);
        Task<T?> GetByIdAsync(string id);
        Task<T?> GetByConditionAsync(Expression<Func<T, bool>> filter);
        Task<T> AddAsync(T entity);
        Task UpdateAsync(string id, T entity);
        Task RemoveAsync(string id);
        Task<bool> AnyAsync(Expression<Func<T, bool>>? filter = null);
        Task<long> CountAsync(Expression<Func<T, bool>>? filter = null);
        Task<List<TResult>> GetWithJoinAsync<TForeign, TResult>(
            Expression<Func<T, bool>>? localFilter,
            Expression<Func<T, string>> localField,
            Expression<Func<TForeign, string>> foreignField,
            Expression<Func<T, TForeign, TResult>> resultSelector)
            where TForeign : class
            where TResult : class;
    }
}
