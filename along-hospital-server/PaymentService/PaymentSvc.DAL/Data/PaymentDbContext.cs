using MongoDB.Driver;
using PaymentSvc.DAL.Models;
using SharedLibrary.Base.Data.MongoDb;

namespace PaymentSvc.DAL.Data
{
    public class PaymentDbContext(IMongoDatabase database) : BaseMongoDbContext(database)
    {
        public IMongoCollection<Payment> Payments => Set<Payment>();
    }
}