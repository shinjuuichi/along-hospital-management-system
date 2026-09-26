using MedicalOrderSvc.DAL.Data;
using MedicalOrderSvc.DAL.Enums;
using MedicalOrderSvc.DAL.Models;
using MedicalOrderSvc.DAL.Models.ClinicalMedicalOrders;
using MedicalOrderSvc.DAL.Models.InfusionMedicalOrders;
using MedicalOrderSvc.DAL.Models.InstructionMedicalOrders;
using MedicalOrderSvc.DAL.Models.Snapshots;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using SharedLibrary.Base.Data.MongoDb;

namespace MedicalOrderSvc.DAL.Seeds
{
    public class MedicalOrderSeed : IMongoDataSeed
    {
        public async Task SeedAsync(IServiceProvider serviceProvider)
        {
            var database = serviceProvider.GetRequiredService<IMongoDatabase>();
            var context = new MedicalOrderDbContext(database);

            var medicalOrderCollection = context.MedicalOrder;
            var medicalOrderCount = await medicalOrderCollection.CountDocumentsAsync(Builders<MedicalOrder>.Filter.Empty);
            if (medicalOrderCount > 0)
            {
                return;
            }

            var defaultMedicalOrders = this.CreateDefaultMedicalOrders();
            await medicalOrderCollection.InsertManyAsync(defaultMedicalOrders);
        }

        private List<MedicalOrder> CreateDefaultMedicalOrders()
        {
            return
            [
                new ClinicalMedicalOrder
                {
                    Id = "64f000000000000000000001",
                    MedicalOrderType = MedicalOrderTypeEnum.Clinical,
                    Instruction = "Order CBC and CRP tests to monitor fever response.",
                    MedicalHistoryId = 1,
                    ClinicalMedicalOrderStatus = ClinicalMedicalOrderStatusEnum.Paid,
                    ClinicalMedicalOrderDetails =
                    [
                        new ClinicalMedicalOrderDetail
                        {
                            Id = "64f000000000000000000001",
                            Quantity = 1,
                            ClinicalMedicalOrderDetailStatus = ClinicalMedicalOrderDetailStatusEnum.Completed,
                            MedicalServiceId = 1,
                            MedicalServiceSnapshot = new MedicalServiceSnapshot
                            {
                                Name = "Complete Blood Count",
                                Description = "Measures red cells, white cells, and platelets.",
                                Code = "LAB-CBC",
                            },
                        },
                        new ClinicalMedicalOrderDetail
                        {
                            Id = "64f000000000000000000002",
                            Quantity = 1,
                            ClinicalMedicalOrderDetailStatus = ClinicalMedicalOrderDetailStatusEnum.Completed,
                            MedicalServiceId = 2,
                            MedicalServiceSnapshot = new MedicalServiceSnapshot
                            {
                                Name = "C-Reactive Protein",
                                Description = "Inflammation marker for infection follow-up.",
                                Code = "LAB-CRP",
                            },
                        },
                    ]
                },
                new InfusionMedicalOrder
                {
                    Id = "64f000000000000000000002",
                    MedicalOrderType = MedicalOrderTypeEnum.Infusion,
                    Instruction = "Hydration and antipyretic infusion for mild fever.",
                    MedicalHistoryId = 1,
                    InfusionMedicalOrderDetails =
                    [
                        new InfusionMedicalOrderDetail
                        {
                            Rate = "80 ml/hour",
                            Frequency = "Every 8 hours",
                            Duration = "24 hours",
                            InfusionMedicalOrderDetailExecutionStatus = InfusionMedicalOrderDetailExecutionStatusEnum.Completed,
                            Note = "Patient tolerated infusion well.",
                            MedicineId = 1,
                            MedicineSnapshot = new MedicineSnapshot
                            {
                                Name = "Paracetamol IV",
                                Brand = "Along Pharma",
                                MedicineUnit = "Vial",
                                MedicineImage = "paracetamol-iv.jpg",
                                CategoryName = "Antipyretic",
                            },
                        }
                    ]
                },
                new ClinicalMedicalOrder
                {
                    Id = "64f000000000000000000003",
                    MedicalOrderType = MedicalOrderTypeEnum.Clinical,
                    Instruction = "Evaluate muscle strain and monitor pain score.",
                    MedicalHistoryId = 2,
                    ClinicalMedicalOrderStatus = ClinicalMedicalOrderStatusEnum.Paid,
                    ClinicalMedicalOrderDetails =
                    [
                        new ClinicalMedicalOrderDetail
                        {
                            Id = "64f000000000000000000003",
                            Quantity = 1,
                            ClinicalMedicalOrderDetailStatus = ClinicalMedicalOrderDetailStatusEnum.Completed,
                            MedicalServiceId = 1,
                            MedicalServiceSnapshot = new MedicalServiceSnapshot
                            {
                                Name = "Complete Blood Count",
                                Description = "Baseline blood test for clinical evaluation.",
                                Code = "LAB-CBC",
                            },
                        },
                        new ClinicalMedicalOrderDetail
                        {
                            Id = "64f000000000000000000004",
                            Quantity = 1,
                            ClinicalMedicalOrderDetailStatus = ClinicalMedicalOrderDetailStatusEnum.Completed,
                            MedicalServiceId = 3,
                            MedicalServiceSnapshot = new MedicalServiceSnapshot
                            {
                                Name = "Musculoskeletal Ultrasound",
                                Description = "Assess soft tissue strain and inflammation.",
                                Code = "IMG-MSK-US",
                            },
                        },
                    ]
                },
                new InfusionMedicalOrder
                {
                    Id = "64f000000000000000000004",
                    MedicalOrderType = MedicalOrderTypeEnum.Infusion,
                    Instruction = "Administer short-course infusion for pain control.",
                    MedicalHistoryId = 2,
                    InfusionMedicalOrderDetails =
                    [
                        new InfusionMedicalOrderDetail
                        {
                            Rate = "60 ml/hour",
                            Frequency = "Twice daily",
                            Duration = "2 days",
                            InfusionMedicalOrderDetailExecutionStatus = InfusionMedicalOrderDetailExecutionStatusEnum.Completed,
                            Note = "Pain reduced from 7/10 to 3/10.",
                            MedicineId = 2,
                            MedicineSnapshot = new MedicineSnapshot
                            {
                                Name = "Ketorolac IV",
                                Brand = "Along Pharma",
                                MedicineUnit = "Ampoule",
                                MedicineImage = "ketorolac-iv.jpg",
                                CategoryName = "NSAID",
                            },
                        }
                    ]
                },
                new ClinicalMedicalOrder
                {
                    Id = "64f000000000000000000005",
                    MedicalOrderType = MedicalOrderTypeEnum.Clinical,
                    Instruction = "Request chest imaging and inflammatory marker panel.",
                    MedicalHistoryId = 3,
                    ClinicalMedicalOrderStatus = ClinicalMedicalOrderStatusEnum.Pending,
                    ClinicalMedicalOrderDetails =
                    [
                        new ClinicalMedicalOrderDetail
                        {
                            Id = "64f000000000000000000005",
                            Quantity = 1,
                            ClinicalMedicalOrderDetailStatus = ClinicalMedicalOrderDetailStatusEnum.Pending,
                            MedicalServiceId = 2,
                            MedicalServiceSnapshot = new MedicalServiceSnapshot
                            {
                                Name = "C-Reactive Protein",
                                Description = "Evaluate active inflammation in respiratory infection.",
                                Code = "LAB-CRP",
                            },
                        }
                    ]
                },
                new InfusionMedicalOrder
                {
                    Id = "64f000000000000000000006",
                    MedicalOrderType = MedicalOrderTypeEnum.Infusion,
                    Instruction = "Start oxygen-supportive infusion protocol and monitor response.",
                    MedicalHistoryId = 3,
                    InfusionMedicalOrderDetails =
                    [
                        new InfusionMedicalOrderDetail
                        {
                            Rate = "90 ml/hour",
                            Frequency = "Continuous",
                            Duration = "72 hours",
                            InfusionMedicalOrderDetailExecutionStatus = InfusionMedicalOrderDetailExecutionStatusEnum.Pending,
                            Note = "Awaiting first execution by nursing team.",
                            MedicineId = 3,
                            MedicineSnapshot = new MedicineSnapshot
                            {
                                Name = "Normal Saline 0.9%",
                                Brand = "Along Med",
                                MedicineUnit = "Bag",
                                MedicineImage = "normal-saline.jpg",
                                CategoryName = "Fluid Therapy",
                            },
                        }
                    ]
                },
                new ClinicalMedicalOrder
                {
                    Id = "64f000000000000000000007",
                    MedicalOrderType = MedicalOrderTypeEnum.Clinical,
                    Instruction = "Post-appendectomy lab follow-up and wound assessment.",
                    MedicalHistoryId = 4,
                    ClinicalMedicalOrderStatus = ClinicalMedicalOrderStatusEnum.Pending,
                    ClinicalMedicalOrderDetails =
                    [
                        new ClinicalMedicalOrderDetail
                        {
                            Id = "64f000000000000000000006",
                            Quantity = 1,
                            ClinicalMedicalOrderDetailStatus = ClinicalMedicalOrderDetailStatusEnum.Pending,
                            MedicalServiceId = 1,
                            MedicalServiceSnapshot = new MedicalServiceSnapshot
                            {
                                Name = "Complete Blood Count",
                                Description = "Postoperative follow-up for infection screening.",
                                Code = "LAB-CBC",
                            },
                        }
                    ]
                },
                new InfusionMedicalOrder
                {
                    Id = "64f000000000000000000008",
                    MedicalOrderType = MedicalOrderTypeEnum.Infusion,
                    Instruction = "Maintain postoperative hydration and antibiotic infusion.",
                    MedicalHistoryId = 4,
                    InfusionMedicalOrderDetails =
                    [
                        new InfusionMedicalOrderDetail
                        {
                            Rate = "70 ml/hour",
                            Frequency = "Every 12 hours",
                            Duration = "48 hours",
                            InfusionMedicalOrderDetailExecutionStatus = InfusionMedicalOrderDetailExecutionStatusEnum.Pending,
                            Note = "Monitor wound pain and hydration balance.",
                            MedicineId = 4,
                            MedicineSnapshot = new MedicineSnapshot
                            {
                                Name = "Ceftriaxone IV",
                                Brand = "Along Pharma",
                                MedicineUnit = "Vial",
                                MedicineImage = "ceftriaxone-iv.jpg",
                                CategoryName = "Antibiotic",
                            },
                        }
                    ]
                },
                new InstructionMedicalOrder
                {
                    Id = "64f000000000000000000009",
                    MedicalOrderType = MedicalOrderTypeEnum.Instruction,
                    InstructionMedicalOrderStatus = InstructionMedicalOrderStatusEnum.Issued,
                    Instruction = "Instruction package for outpatient fever monitoring.",
                    MedicalHistoryId = 1,
                    PositionOrder = new PositionOrder
                    {
                        PositionOrderType = PositionOrderTypeEnum.HeadElevated30,
                        Instruction = "Rest with head elevated at 30 degrees after meals.",
                    },
                    NutritionOrder = new NutritionOrder
                    {
                        NutritionOrderType = NutritionOrderTypeEnum.SoftRice,
                        Instruction = "Take warm, soft diet and increase oral fluids.",
                    },
                },
                new InstructionMedicalOrder
                {
                    Id = "64f00000000000000000000a",
                    MedicalOrderType = MedicalOrderTypeEnum.Instruction,
                    InstructionMedicalOrderStatus = InstructionMedicalOrderStatusEnum.Issued,
                    Instruction = "Instruction package for muscle recovery.",
                    MedicalHistoryId = 2,
                    PositionOrder = new PositionOrder
                    {
                        PositionOrderType = PositionOrderTypeEnum.Supine,
                        Instruction = "Limit abrupt posture changes during first 24 hours.",
                    },
                },
                new InstructionMedicalOrder
                {
                    Id = "64f00000000000000000000b",
                    MedicalOrderType = MedicalOrderTypeEnum.Instruction,
                    InstructionMedicalOrderStatus = InstructionMedicalOrderStatusEnum.Issued,
                    Instruction = "Comprehensive inpatient instruction for pneumonia management.",
                    MedicalHistoryId = 3,
                    PositionOrder = new PositionOrder
                    {
                        PositionOrderType = PositionOrderTypeEnum.HeadElevated45,
                        Instruction = "Maintain Fowler position to improve lung expansion.",
                    },
                    RespiratorySupportOrder = new RespiratorySupportOrder
                    {
                        RespiratorySupportOrderType = RespiratorySupportOrderTypeEnum.OxygenNasalCannula,
                        OxygenFlow = 2.5,
                        FiO2 = 0.32,
                        Instruction = "Adjust oxygen flow to maintain SpO2 at or above 94%.",
                    },
                    NutritionOrder = new NutritionOrder
                    {
                        NutritionOrderType = NutritionOrderTypeEnum.SoftRice,
                        Instruction = "Small frequent meals, avoid cold beverages.",
                    },
                    NursingCareOrder = new NursingCareOrder
                    {
                        NursingCareOrderLevel = NursingCareOrderLevelEnum.Level3,
                        MonitorIntervalHour = 2,
                    },
                },
                new InstructionMedicalOrder
                {
                    Id = "64f00000000000000000000c",
                    MedicalOrderType = MedicalOrderTypeEnum.Instruction,
                    InstructionMedicalOrderStatus = InstructionMedicalOrderStatusEnum.Issued,
                    Instruction = "Postoperative appendectomy nursing instruction.",
                    MedicalHistoryId = 4,
                    NursingCareOrder = new NursingCareOrder
                    {
                        NursingCareOrderLevel = NursingCareOrderLevelEnum.Level2,
                        MonitorIntervalHour = 4,
                    },
                },
            ];
        }
    }
}