using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using SharedLibrary.Base.Data.MongoDb;
using VoucherSvc.DAL.Data;
using VoucherSvc.DAL.Enums;
using VoucherSvc.DAL.Models;

namespace VoucherSvc.DAL.Seeds
{
    public class VoucherSeed : IMongoDataSeed
    {
        public async Task SeedAsync(IServiceProvider serviceProvider)
        {
            var database = serviceProvider.GetRequiredService<IMongoDatabase>();
            var context = new VoucherDbContext(database);

            var voucherCollection = context.Voucher;
            var voucherCount = await voucherCollection.CountDocumentsAsync(Builders<Voucher>.Filter.Empty);

            if (voucherCount > 0)
            {
                return;
            }

            var defaultVouchers = CreateDefaultVouchers();
            await voucherCollection.InsertManyAsync(defaultVouchers);
        }

        private static List<Voucher> CreateDefaultVouchers()
        {
            var now = DateOnly.FromDateTime(DateTime.Now);
            return
                [
                    new() {
                        Name = "WELCOME10",
                        Description = "10% off your order (min $50).",
                        Code = "PV-WELCOME10",
                        VoucherType = VoucherTypeEnum.Patient,
                        VoucherStatus = VoucherStatusEnum.Active,
                        ExpireDate = now.AddDays(60),
                        Quantity = 1000,
                        DiscountType = DiscountTypeEnum.Percentage,
                        DiscountValue = 10,
                        MaxDiscount = 20,
                        MinPurchaseAmount = 50,
                    },
                    new() {
                        Name = "FIRSTORDER5",
                        Description = "$5 off your first order.",
                        Code = "PV-FIRST5",
                        VoucherType = VoucherTypeEnum.Patient,
                        VoucherStatus = VoucherStatusEnum.Active,
                        ExpireDate = now.AddDays(60),
                        Quantity = 1000,
                        DiscountType = DiscountTypeEnum.FixedAmount,
                        DiscountValue = 5,
                    },

                    new MedicineDiscount {
                        Name = "PAINRELIEF15",
                        Description = "15% off selected medicines.",
                        Code = "MD-PAIN15",
                        VoucherType = VoucherTypeEnum.Medicine,
                        VoucherStatus = VoucherStatusEnum.Active,
                        ExpireDate = now.AddDays(60),
                        Quantity = 500,
                        DiscountType = DiscountTypeEnum.Percentage,
                        DiscountValue = 15,
                        MedicineIds = [1, 2]
                    },
                    new MedicineDiscount {
                        Name = "WEEKEND20",
                        Description = "20% off selected medicines this weekend.",
                        Code = "MD-WEEKEND20",
                        VoucherType = VoucherTypeEnum.Medicine,
                        VoucherStatus = VoucherStatusEnum.Active,
                        ExpireDate = now.AddDays(7),
                        Quantity = 300,
                        DiscountType = DiscountTypeEnum.Percentage,
                        DiscountValue = 20,
                        MedicineIds = [1, 2, 3]
                    }
                ];
        }
    }
}
