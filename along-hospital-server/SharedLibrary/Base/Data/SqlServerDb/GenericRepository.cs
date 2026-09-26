using Gridify;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Commons.EntityAbstractions;
using SharedLibrary.Services.Interfaces;
using System.Data;
using System.Linq.Expressions;

namespace SharedLibrary.Base.Data.SqlServerDb
{
    public class GenericRepository<T>(DbContext dbContext, ICurrentUserService currentUserService)
            : IGenericRepository<T> where T : Entity
    {
        protected readonly DbContext _dbContext = dbContext;
        protected readonly ICurrentUserService _currentUserService = currentUserService;

        #region Get All
        public async Task<List<T>> GetAllAsync(
            Expression<Func<T, bool>>? filter = null,
            string[]? includes = null,
            CancellationToken cancellationToken = default)
        {
            IQueryable<T> query = _dbContext.Set<T>().Where(filter ?? (_ => true));
            query = ApplyIncludes(query, includes);
            return await query.AsNoTracking().ToListAsync(cancellationToken);
        }

        public IQueryable<T> GetAllQueryable(string[]? includes = null)
        {
            IQueryable<T> query = _dbContext.Set<T>();
            query = ApplyIncludes(query, includes);
            return query.AsNoTracking();
        }

        public async Task<List<T>> GetAllByIdsAsync(List<int> ids, string[]? includes = null,
                                                   CancellationToken cancellationToken = default)
        {
            if (ids.Count == 0 || !typeof(BaseEntity).IsAssignableFrom(typeof(T)))
            {
                return [];
            }

            IQueryable<T> query = _dbContext.Set<T>();
            query = ApplyIncludes(query, includes);

            if (ids.Count == 1)
            {
                int id = ids[0];

                var entity = await query
                    .AsNoTracking()
                    .FirstOrDefaultAsync(e => (e as BaseEntity)!.Id == id, cancellationToken);

                return entity != null ? [entity] : [];
            }

            return await query
                .Where(e => ids.Contains((e as BaseEntity)!.Id))
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
        #endregion

        #region Get All Paginated
        public async Task<(int Count, List<T> Items)> GetAllPaginatedAsync(string? filter,
                                                               string order,
                                                               int page,
                                                               int pageSize,
                                                               string[]? includes = null,
                                                               CancellationToken cancellationToken = default)
        {
            page = Math.Max(page, 1);
            pageSize = Math.Clamp(pageSize, 1, 100);

            var query = this.GetAllQueryable(includes);
            var total = query.Count();
            try
            {
                query = query.ApplyFiltering(filter).ApplyOrdering(order);
                total = query.Count();
            }
            finally
            {
                query = query.ApplyPaging(page, pageSize);
            }

            return (total, await query.ToListAsync(cancellationToken));
        }

        public async Task<(int Count, List<T> Items)> GetAllPaginatedAsync(Expression<Func<T, bool>>? filter,
                                                               string order,
                                                               int page,
                                                               int pageSize,
                                                               string[]? includes = null,
                                                               CancellationToken cancellationToken = default)
        {
            page = Math.Max(page, 1);
            pageSize = Math.Clamp(pageSize, 1, 100);

            var query = this.GetAllQueryable(includes).Where(filter ?? (_ => true));
            var total = query.Count();
            try
            {
                query = query.ApplyOrdering(order);
            }
            finally
            {
                query = query.ApplyPaging(page, pageSize);
            }

            return (total, await query.ToListAsync(cancellationToken));
        }

        public async Task<(int Count, List<T> Items)> GetAllPaginatedAsync(Expression<Func<T, bool>>? filterExpr,
                                                               string? filterStr,
                                                               string order,
                                                               int page,
                                                               int pageSize,
                                                               string[]? includes = null,
                                                               CancellationToken cancellationToken = default)
        {
            page = Math.Max(page, 1);
            pageSize = Math.Clamp(pageSize, 1, 100);

            var query = this.GetAllQueryable(includes).Where(filterExpr ?? (_ => true));
            var total = query.Count();
            try
            {
                query = query.ApplyFiltering(filterStr).ApplyOrdering(order);
                total = query.Count();
            }
            finally
            {
                query = query.ApplyPaging(page, pageSize);
            }

            return (total, await query.ToListAsync(cancellationToken));
        }
        #endregion

        #region Get One
        public async Task<T?> GetByIdAsync(int id, string[]? includes = null,
                                         CancellationToken cancellationToken = default)
        {
            if (!typeof(BaseEntity).IsAssignableFrom(typeof(T)))
            {
                return null;
            }

            if (includes == null || includes.Length == 0)
            {
                return await _dbContext.Set<T>().FindAsync(id, cancellationToken);
            }

            IQueryable<T> query = _dbContext.Set<T>();
            query = ApplyIncludes(query, includes);
            return await query.FirstOrDefaultAsync(entity => (entity as BaseEntity)!.Id == id, cancellationToken);
        }

        public async Task<T?> GetByConditionAsync(Expression<Func<T, bool>> filter, string[]? includes = null,
                                                 CancellationToken cancellationToken = default)
        {
            IQueryable<T> query = _dbContext.Set<T>();
            query = ApplyIncludes(query, includes);
            return await query.FirstOrDefaultAsync(filter, cancellationToken);
        }
        #endregion

        #region Add
        public async Task<T> AddAsync(T entity,
                                     CancellationToken cancellationToken = default)
        {
            if (entity is AuditEntity auditEntity)
            {
                auditEntity.CreatedBy = _currentUserService.UserId;
            }
            var result = await _dbContext.Set<T>().AddAsync(entity, cancellationToken);
            return result.Entity;
        }

        public async Task AddRangeAsync(List<T> entities,
                                         CancellationToken cancellationToken = default)
        {
            foreach (var entity in entities)
            {
                if (entity is AuditEntity auditEntity)
                {
                    auditEntity.CreatedBy = _currentUserService.UserId;
                }
            }
            await _dbContext.Set<T>().AddRangeAsync(entities, cancellationToken);
        }
        #endregion

        #region Remove
        public void Remove(T entity)
        {
            if (entity is AuditEntity auditableEntity)
            {
                auditableEntity.DeletionDate = DateTime.UtcNow;
                auditableEntity.IsDeleted = true;
                _dbContext.Set<T>().Update(entity);
            }
            else
            {
                _dbContext.Set<T>().Remove(entity);
            }
        }

        public void RemoveRange(List<T> entities)
        {
            if (typeof(AuditEntity).IsAssignableFrom(typeof(T)))
            {
                foreach (var entity in entities)
                {
                    if (entity is AuditEntity auditableEntity)
                    {
                        auditableEntity.DeletionDate = DateTime.UtcNow;
                        auditableEntity.IsDeleted = true;
                    }
                }
                _dbContext.Set<T>().UpdateRange(entities);
            }
            else
            {
                _dbContext.Set<T>().RemoveRange(entities);
            }
        }
        #endregion

        #region Update
        public T Update(T entity)
        {
            if (entity is AuditEntity auditableEntity)
            {
                auditableEntity.ModificationDate = DateTime.UtcNow;
                auditableEntity.ModifiedBy = _currentUserService.UserId;
            }

            var result = _dbContext.Set<T>().Update(entity);
            return result.Entity;
        }

        public void UpdateRange(List<T> entities)
        {
            foreach (var entity in entities)
            {
                if (entity is AuditEntity auditableEntity)
                {
                    auditableEntity.ModificationDate = DateTime.UtcNow;
                    auditableEntity.ModifiedBy = _currentUserService.UserId;
                }
            }
            _dbContext.Set<T>().UpdateRange(entities);
        }
        #endregion

        #region Others
        public async Task<bool> AnyAsync(Expression<Func<T, bool>>? filter = null,
                                         CancellationToken cancellationToken = default)
            => await _dbContext.Set<T>().AnyAsync(filter ?? (_ => true), cancellationToken);

        public async Task<int> CountAsync(Expression<Func<T, bool>>? filter = null,
                                           CancellationToken cancellationToken = default)
            => await _dbContext.Set<T>().CountAsync(filter ?? (_ => true), cancellationToken);
        #endregion

        private IQueryable<T> ApplyIncludes(IQueryable<T> query, string[]? _includes)
        {
            if (_includes != null)
            {
                foreach (var include in _includes)
                {
                    query = query.Include(include);
                }
            }
            return query;
        }
    }
}