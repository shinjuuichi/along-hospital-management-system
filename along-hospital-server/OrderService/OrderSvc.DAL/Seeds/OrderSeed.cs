using Microsoft.EntityFrameworkCore;
using OrderSvc.DAL.Enums;
using OrderSvc.DAL.Models;
using OrderSvc.DAL.Models.Snapshots;
using SharedLibrary.Base.Data;

namespace OrderSvc.DAL.Seeds
{
    public class OrderSeed : ISeedBuilder
    {
        public int Priority => 10;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>().HasData(
                new Order
                {
                    Id = 1,
                    PatientId = 3,
                    OrderDate = new DateTime(2025, 3, 28),
                    CreationDate = new DateTime(2025, 3, 28),
                    IsDeleted = false,
                    OrderStatus = OrderStatusEnum.Unpaid,
                    IsPickupAtStore = false,
                    OriginPrice = 5.50,
                    TotalDiscountAmount = 0,
                    FinalPrice = 5.50,
                    VoucherCode = null,
                    TransactionId = null,
                },
                new Order
                {
                    Id = 2,
                    PatientId = 3,
                    OrderDate = new DateTime(2025, 3, 26),
                    PaidDate = new DateTime(2025, 3, 26),
                    CreationDate = new DateTime(2025, 3, 26),
                    IsDeleted = false,
                    OrderStatus = OrderStatusEnum.Paid,
                    IsPickupAtStore = true,
                    OriginPrice = 3.00,
                    TotalDiscountAmount = 0.45,
                    FinalPrice = 2.55,
                    VoucherCode = null,
                    TransactionId = null,
                },
                new Order
                {
                    Id = 3,
                    PatientId = 3,
                    OrderDate = new DateTime(2025, 3, 21),
                    PaidDate = new DateTime(2025, 3, 21),
                    DeliveryDate = new DateTime(2025, 3, 23),
                    CreationDate = new DateTime(2025, 3, 21),
                    IsDeleted = false,
                    OrderStatus = OrderStatusEnum.Completed,
                    IsPickupAtStore = false,
                    OriginPrice = 8.00,
                    TotalDiscountAmount = 0.90,
                    FinalPrice = 7.10,
                    VoucherCode = "PV-WELCOME10",
                    TransactionId = null,
                },
                new Order
                {
                    Id = 4,
                    PatientId = 3,
                    OrderDate = new DateTime(2025, 3, 24),
                    CreationDate = new DateTime(2025, 3, 24),
                    IsDeleted = false,
                    OrderStatus = OrderStatusEnum.Cancelled,
                    IsPickupAtStore = true,
                    OriginPrice = 1.20,
                    TotalDiscountAmount = 0,
                    FinalPrice = 1.20,
                    VoucherCode = null,
                    TransactionId = null,
                },
                new Order
                {
                    Id = 5,
                    PatientId = 3,
                    OrderDate = new DateTime(2025, 3, 30),
                    PaidDate = new DateTime(2025, 3, 30),
                    CreationDate = new DateTime(2025, 3, 30),
                    IsDeleted = false,
                    OrderStatus = OrderStatusEnum.Shipping,
                    IsPickupAtStore = false,
                    OriginPrice = 4.50,
                    TotalDiscountAmount = 0.45,
                    FinalPrice = 4.05,
                    VoucherCode = null,
                    TransactionId = null,
                },
                new Order
                {
                    Id = 6,
                    PatientId = 3,
                    OrderDate = new DateTime(2025, 3, 27),
                    PaidDate = new DateTime(2025, 3, 27),
                    CreationDate = new DateTime(2025, 3, 27),
                    IsDeleted = false,
                    OrderStatus = OrderStatusEnum.Paid,
                    IsPickupAtStore = true,
                    OriginPrice = 3.00,
                    TotalDiscountAmount = 3.00,
                    FinalPrice = 0,
                    VoucherCode = "PV-FIRST5",
                    TransactionId = null,
                }
            );

            modelBuilder.Entity<OrderDetail>().HasData(
                new OrderDetail
                {
                    OrderId = 1,
                    SKUCode = "PA1BO2050",
                    Quantity = 2,
                    UnitPrice = 1.50,
                    DiscountAmount = 0,
                    MedicineSnapshot = new MedicineSnapshot
                    {
                        MedicineName = "Paracetamol 500",
                        MedicineBrand = "Stada",
                        MedicineImages = ["Medicine/thuoc-paracetamol-650-mg-mediplantex-3-c1880_1770294316_bdee440d.webp"],
                        MedicineUnit = "Tablet"
                    }
                },
                new OrderDetail
                {
                    OrderId = 1,
                    SKUCode = "AM2BO2050",
                    Quantity = 1,
                    UnitPrice = 2.50,
                    DiscountAmount = 0,
                    MedicineSnapshot = new MedicineSnapshot
                    {
                        MedicineName = "Amoxicillin 500",
                        MedicineBrand = "Medipharma",
                        MedicineImages = ["Medicine/amoxicillin_1770294305_24f46c57.webp"],
                        MedicineUnit = "Capsule"
                    }
                },
                new OrderDetail
                {
                    OrderId = 2,
                    SKUCode = "VC3BO2050",
                    Quantity = 1,
                    UnitPrice = 3.00,
                    DiscountAmount = 0.45,
                    MedicineSnapshot = new MedicineSnapshot
                    {
                        MedicineName = "Vitamin C 1000",
                        MedicineBrand = "Nature's Way",
                        MedicineImages = ["Medicine/VitaminC_1770294282_a543fa89.webp"],
                        MedicineUnit = "Effervescent"
                    }
                },
                new OrderDetail
                {
                    OrderId = 3,
                    SKUCode = "PA1BO3050",
                    Quantity = 1,
                    UnitPrice = 2.00,
                    DiscountAmount = 0,
                    MedicineSnapshot = new MedicineSnapshot
                    {
                        MedicineName = "Paracetamol 500",
                        MedicineBrand = "Stada",
                        MedicineImages = ["Medicine/thuoc-paracetamol-650-mg-mediplantex-3-c1880_1770294316_bdee440d.webp"],
                        MedicineUnit = "Tablet"
                    }
                },
                new OrderDetail
                {
                    OrderId = 3,
                    SKUCode = "VC3BO2050",
                    Quantity = 2,
                    UnitPrice = 3.00,
                    DiscountAmount = 0.45,
                    MedicineSnapshot = new MedicineSnapshot
                    {
                        MedicineName = "Vitamin C 1000",
                        MedicineBrand = "Nature's Way",
                        MedicineImages = ["Medicine/VitaminC_1770294282_a543fa89.webp"],
                        MedicineUnit = "Effervescent"
                    }
                },
                new OrderDetail
                {
                    OrderId = 4,
                    SKUCode = "AM2BO1050",
                    Quantity = 1,
                    UnitPrice = 1.20,
                    DiscountAmount = 0,
                    MedicineSnapshot = new MedicineSnapshot
                    {
                        MedicineName = "Amoxicillin 500",
                        MedicineBrand = "Medipharma",
                        MedicineImages = ["Medicine/amoxicillin_1770294305_24f46c57.webp"],
                        MedicineUnit = "Capsule"
                    }
                },
                new OrderDetail
                {
                    OrderId = 5,
                    SKUCode = "PA1BO2050",
                    Quantity = 1,
                    UnitPrice = 1.50,
                    DiscountAmount = 0,
                    MedicineSnapshot = new MedicineSnapshot
                    {
                        MedicineName = "Paracetamol 500",
                        MedicineBrand = "Stada",
                        MedicineImages = ["Medicine/thuoc-paracetamol-650-mg-mediplantex-3-c1880_1770294316_bdee440d.webp"],
                        MedicineUnit = "Tablet"
                    }
                },
                new OrderDetail
                {
                    OrderId = 5,
                    SKUCode = "VC3BO2050",
                    Quantity = 1,
                    UnitPrice = 3.00,
                    DiscountAmount = 0.45,
                    MedicineSnapshot = new MedicineSnapshot
                    {
                        MedicineName = "Vitamin C 1000",
                        MedicineBrand = "Nature's Way",
                        MedicineImages = ["Medicine/VitaminC_1770294282_a543fa89.webp"],
                        MedicineUnit = "Effervescent"
                    }
                },
                new OrderDetail
                {
                    OrderId = 6,
                    SKUCode = "VC3BO2050",
                    Quantity = 1,
                    UnitPrice = 3.00,
                    DiscountAmount = 3.00,
                    MedicineSnapshot = new MedicineSnapshot
                    {
                        MedicineName = "Vitamin C 1000",
                        MedicineBrand = "Nature's Way",
                        MedicineImages = ["Medicine/VitaminC_1770294282_a543fa89.webp"],
                        MedicineUnit = "Effervescent"
                    }
                }
            );

            return modelBuilder;
        }
    }
}