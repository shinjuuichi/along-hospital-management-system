using MongoDB.Driver;
using SharedLibrary.Commons.EntityAbstractions.Mongo;
using SharedLibrary.Commons.Filters;
using SharedLibrary.Services.Interfaces;
using System.Linq.Expressions;

namespace SharedLibrary.Base.Data.MongoDb
{
    public class MongoGenericRepository<T>(BaseMongoDbContext context, ICurrentUserService currentUserService) : IMongoGenericRepository<T> where T : MongoBaseEntity
    {
        protected readonly BaseMongoDbContext context = context;
        protected readonly IMongoCollection<T> _collection = context.Set<T>();
        protected readonly ICurrentUserService _currentUserService = currentUserService;
        #region Get
        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null)
        {
            var appliedFilter = this.BuildReadFilter(filter);
            return await _collection.Find(appliedFilter).ToListAsync();
        }

        public async Task<T?> GetByIdAsync(string id)
        {
            var appliedFilter = this.BuildReadFilter(Builders<T>.Filter.Eq(x => x.Id, id));
            return await _collection
                .Find(appliedFilter)
                .FirstOrDefaultAsync();
        }

        public async Task<T?> GetByConditionAsync(Expression<Func<T, bool>> filter)
        {
            var appliedFilter = this.BuildReadFilter(filter);
            return await _collection.Find(appliedFilter).FirstOrDefaultAsync();
        }

        public async Task<(int Count, List<T> Items)> GetAllPaginatedAsync(MongoFilterDTO filterDTO)
        {
            var page = Math.Max(filterDTO.Page, 1);
            var pageSize = Math.Clamp(filterDTO.PageSize, 1, 100);

            var filter = filterDTO.BuildFilter<T>();
            var appliedFilter = this.BuildReadFilter(filter);

            var findQuery = _collection.Find(appliedFilter);

            var sort = filterDTO.BuildSort<T>();
            if (sort != null)
            {
                findQuery = findQuery.Sort(sort);
            }
            else
            {
                findQuery = findQuery.Sort(Builders<T>.Sort.Descending("_id"));
            }

            var total = (int)await _collection.CountDocumentsAsync(appliedFilter);
            var items = await findQuery
                .Skip((page - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync();

            return (total, items);
        }
        #endregion

        #region Write
        public async Task<T> AddAsync(T entity)
        {
            if (entity is MongoAuditEntity auditableEntity)
            {
                var userId = _currentUserService.UserId;
                auditableEntity.CreatedBy = userId;
                auditableEntity.CreationDate = DateTime.UtcNow;
            }

            await _collection.InsertOneAsync(entity);
            return entity;
        }

        public async Task UpdateAsync(string id, T entity)
        {
            if (entity is MongoAuditEntity auditableEntity)
            {
                var userId = _currentUserService.UserId;
                auditableEntity.ModifiedBy = userId;
                auditableEntity.ModificationDate = DateTime.UtcNow;
            }

            entity.Id = id;

            var filter = Builders<T>.Filter.Eq(x => x.Id, id);
            await _collection.ReplaceOneAsync(filter, entity);
        }

        public async Task RemoveAsync(string id)
        {
            var filter = Builders<T>.Filter.Eq(x => x.Id, id);

            if (typeof(MongoAuditEntity).IsAssignableFrom(typeof(T)))
            {
                var userId = _currentUserService.UserId;
                var update = Builders<T>.Update
                    .Set("IsDeleted", true)
                    .Set("DeletionDate", DateTime.UtcNow)
                    .Set("ModifiedBy", userId);
                await _collection.UpdateOneAsync(filter, update);
            }
            else
            {
                await _collection.DeleteOneAsync(filter);
            }
        }
        #endregion

        #region Others
        public async Task<bool> AnyAsync(Expression<Func<T, bool>>? filter = null)
        {
            var appliedFilter = this.BuildReadFilter(filter);
            return await _collection.Find(appliedFilter).AnyAsync();
        }

        public async Task<long> CountAsync(Expression<Func<T, bool>>? filter = null)
        {
            var appliedFilter = this.BuildReadFilter(filter);
            return await _collection.CountDocumentsAsync(appliedFilter);
        }

        public async Task<List<TResult>> GetWithJoinAsync<TForeign, TResult>(
            Expression<Func<T, bool>>? localFilter,
            Expression<Func<T, string>> localField,
            Expression<Func<TForeign, string>> foreignField,
            Expression<Func<T, TForeign, TResult>> resultSelector)
            where TForeign : class
            where TResult : class
        {
            var localItems = await GetAllAsync(localFilter);

            if (!localItems.Any())
            {
                return new List<TResult>();
            }

            var localFieldFunc = localField.Compile();
            var foreignIds = localItems
                .Select(localFieldFunc)
                .Distinct()
                .ToList();

            var foreignFieldFunc = foreignField.Compile();
            var foreignCollection = context.Set<TForeign>();
            var foreignItems = await foreignCollection
                .Find(this.BuildReadFilter<TForeign>())
                .ToListAsync();

            var foreignDict = foreignItems
                .Where(f => foreignIds.Contains(foreignFieldFunc(f)))
                .ToDictionary(foreignFieldFunc, f => f);

            var resultSelectorFunc = resultSelector.Compile();
            var results = new List<TResult>();

            foreach (var localItem in localItems)
            {
                var foreignKey = localFieldFunc(localItem);
                if (foreignDict.TryGetValue(foreignKey, out var foreignItem))
                {
                    var result = resultSelectorFunc(localItem, foreignItem);
                    results.Add(result);
                }
            }

            return results;
        }

        private FilterDefinition<TDocument> BuildReadFilter<TDocument>()
            where TDocument : class
        {
            return this.BuildReadFilter((FilterDefinition<TDocument>?)null);
        }

        private FilterDefinition<TDocument> BuildReadFilter<TDocument>(Expression<Func<TDocument, bool>>? filter)
            where TDocument : class
        {
            var baseFilter = filter == null
                ? null
                : Builders<TDocument>.Filter.Where(filter);

            return this.BuildReadFilter(baseFilter);
        }

        private FilterDefinition<TDocument> BuildReadFilter<TDocument>(FilterDefinition<TDocument>? filter)
            where TDocument : class
        {
            var builder = Builders<TDocument>.Filter;

            if (!typeof(MongoAuditEntity).IsAssignableFrom(typeof(TDocument)))
            {
                return filter ?? builder.Empty;
            }

            var isDeletedFilter = builder.Eq("IsDeleted", false);
            return filter == null
                ? isDeletedFilter
                : builder.And(isDeletedFilter, filter);
        }
        #endregion
    }
}
