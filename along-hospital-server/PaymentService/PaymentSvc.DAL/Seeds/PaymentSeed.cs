using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using PaymentSvc.DAL.Enums;
using PaymentSvc.DAL.Models;
using SharedLibrary.Base.Data.MongoDb;

namespace PaymentSvc.DAL.Seeds
{
    public class PaymentSeed : IMongoDataSeed
    {
        public async Task SeedAsync(IServiceProvider serviceProvider)
        {
            var database = serviceProvider.GetRequiredService<IMongoDatabase>();
            var collection = database.GetCollection<Payment>("Payments");

            var count = await collection.CountDocumentsAsync(Builders<Payment>.Filter.Empty);

            if (count > 0)
            {
                return;
            }

            var payments = CreateDefaultPayments();
            await collection.InsertManyAsync(payments);
        }

        private static List<Payment> CreateDefaultPayments()
        {
            return
            [
                new PayOSPayment
                {
                    Status = PaymentStatusEnum.Success,
                    OriginalAmount = 120,
                    FinalAmount = 108,
                    Description = "Payment for medical consultation",
                    PaymentItems =
                    [
                        new PaymentItem
                        {
                            ServiceName = "Doctor Consultation",
                            Quantity = 1,
                            UnitPrice = 120
                        }
                    ]
                },

                new CashPayment
                {
                    Status = PaymentStatusEnum.Success,
                    OriginalAmount = 50,
                    FinalAmount = 50,
                    Description = "Cash payment for medicine",
                    PaymentItems =
                    [
                        new PaymentItem
                        {
                            ServiceName = "Painkiller Medicine",
                            Quantity = 2,
                            UnitPrice = 25
                        }
                    ]
                },

                new SePayPayment
                {
                    Status = PaymentStatusEnum.Failed,
                    OriginalAmount = 75,
                    FinalAmount = 82.5,
                    Description = "SePay payment for X-ray service",
                    PaymentItems =
                    [
                        new PaymentItem
                        {
                            ServiceName = "X-ray Imaging",
                            Quantity = 1,
                            UnitPrice = 75
                        }
                    ]
                }
            ];
        }
    }
}