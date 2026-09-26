using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using SharedLibrary.Base.Data.SqlServerDb;
using SharedLibrary.Commons.EntityAbstractions;
using SharedLibrary.Services.Interfaces;

namespace SharedLibrary.Base.Data
{
    public class UnitOfWork(DbContext dbContext, ICurrentUserService currentUserService)
             : IUnitOfWork
    {
        private readonly DbContext _dbContext = dbContext;
        private readonly ICurrentUserService _currentUserService = currentUserService;
        private readonly Dictionary<Type, dynamic> _repositories = [];
        private IDbContextTransaction? _currentTransaction;

        #region Repository
        public IGenericRepository<T> Repository<T>() where T : Entity
        {
            var entityType = typeof(T);

            if (_repositories.TryGetValue(entityType, out dynamic? repository))
            {
                return repository;
            }

            var newRepository = Activator.CreateInstance(
                typeof(GenericRepository<>).MakeGenericType(typeof(T)),
                _dbContext,
                _currentUserService
            );

            if (newRepository == null)
            {
                throw new NullReferenceException("Repository should not be null");
            }

            _repositories.Add(entityType, newRepository);

            return (IGenericRepository<T>)newRepository;
        }
        #endregion

        #region Save Changes
        public async Task<int> SaveChangeAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }
        #endregion

        #region Manual Transaction
        public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_currentTransaction != null)
            {
                throw new InvalidOperationException("A transaction is already active. Nested transactions are not supported.");
            }

            _currentTransaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
        }

        public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_currentTransaction == null)
            {
                return;
            }

            try
            {
                await _currentTransaction.CommitAsync(cancellationToken);
            }
            catch
            {
                await this.RollbackTransactionAsync(cancellationToken);
            }
            finally
            {
                await _currentTransaction.DisposeAsync();
                _currentTransaction = null;
            }
        }

        public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_currentTransaction == null)
            {
                return;
            }

            try
            {
                await _currentTransaction.RollbackAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                throw new Exception("Transaction rollback failed.", ex);
            }
            finally
            {
                await _currentTransaction.DisposeAsync();
                _currentTransaction = null;
            }
        }
        #endregion
    }
}