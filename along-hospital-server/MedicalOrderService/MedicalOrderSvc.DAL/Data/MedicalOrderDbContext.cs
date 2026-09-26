using MedicalOrderSvc.DAL.Models;
using MongoDB.Driver;
using SharedLibrary.Base.Data.MongoDb;

namespace MedicalOrderSvc.DAL.Data
{
    public class MedicalOrderDbContext(IMongoDatabase database) : BaseMongoDbContext(database)
    {
        public IMongoCollection<MedicalOrder> MedicalOrder => Set<MedicalOrder>();
    }
}