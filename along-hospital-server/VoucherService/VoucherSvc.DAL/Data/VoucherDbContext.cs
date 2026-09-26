using MongoDB.Driver;
using SharedLibrary.Base.Data.MongoDb;
using VoucherSvc.DAL.Models;

namespace VoucherSvc.DAL.Data
{
    public class VoucherDbContext(IMongoDatabase database) : BaseMongoDbContext(database)
    {
        public IMongoCollection<PatientVoucher> PatientVoucher => Set<PatientVoucher>();
        public IMongoCollection<Voucher> Voucher => Set<Voucher>();
    }
}
