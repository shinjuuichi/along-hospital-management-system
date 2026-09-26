using System.Reflection;
using MongoDB.Driver;
using SharedLibrary.Commons.EntityAnnotations;

namespace SharedLibrary.Base.Data.MongoDb
{
    public abstract class BaseMongoDbContext
    {
        protected readonly IMongoDatabase _database;

        protected BaseMongoDbContext(IMongoDatabase database)
        {
            _database = database;
            ConfigureCollections();
        }

        private void ConfigureCollections()
        {
            var entityTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a =>
                {
                    try { return a.GetTypes(); } catch { return []; }
                })
                .Where(t => t.IsClass && !t.IsAbstract)
                .ToList();

            foreach (var entityType in entityTypes)
            {
                var collectionName = entityType.GetCustomAttribute<CollectionNameAttribute>()?.Name
                                     ?? entityType.Name;

                var collection = _database.GetCollection<dynamic>(collectionName);
                var indexes = new List<CreateIndexModel<dynamic>>();

                var uniqueProps = entityType.GetProperties()
                    .Where(p => p.GetCustomAttributes<UniqueAttribute>(false).Any());

                foreach (var prop in uniqueProps)
                {
                    var indexKey = Builders<dynamic>.IndexKeys.Ascending(prop.Name);
                    var options = new CreateIndexOptions { Unique = true };
                    indexes.Add(new CreateIndexModel<dynamic>(indexKey, options));
                }

                try
                {
                    if (indexes.Count > 0)
                    {
                        collection.Indexes.CreateMany(indexes);
                    }
                }
                catch
                {
                    Console.WriteLine($"[MONGO INDEX ERROR] Failed to create indexes for collection: {collectionName}");
                }
            }
        }

        public IMongoCollection<T> Set<T>() where T : class
        {
            var collectionName = typeof(T).GetCustomAttribute<CollectionNameAttribute>()?.Name
                                 ?? typeof(T).Name;
            return _database.GetCollection<T>(collectionName);
        }
    }
}