using SharedLibrary.Base.Data.SqlServerDb;
using SharedLibrary.Commons.EntityAbstractions;

namespace SharedLibrary.Base.Data
{
    public interface IUnitOfWork
    {
        IGenericRepository<T> Repository<T>() where T : Entity;

        Task<int> SaveChangeAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Khi bạn cần nhiều bước phức tạp và điều kiện commit tùy logic
        /// </summary>
        Task BeginTransactionAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Commit transaction hiện tại
        /// </summary>
        Task CommitTransactionAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Rollback transaction hiện tại
        /// </summary>
        Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
    }
}