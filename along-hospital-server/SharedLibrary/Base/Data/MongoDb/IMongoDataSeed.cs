namespace SharedLibrary.Base.Data.MongoDb
{
    public interface IMongoDataSeed
    {
        Task SeedAsync(IServiceProvider serviceProvider);
    }
}
