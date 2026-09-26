using Microsoft.EntityFrameworkCore;
using PayrollSvc.DAL.Enums;
using PayrollSvc.DAL.Models;
using PayrollSvc.DAL.Models.Snapshot;
using SharedLibrary.Base.Data;

namespace PayrollSvc.DAL.Seeds
{
    public class PayrollSeed : ISeedBuilder
    {
        public int Priority => 3;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            var seedDate = new DateTime(2025, 1, 31, 0, 0, 0, DateTimeKind.Utc);
            var payrolls = new List<Payroll>
            {
                new Payroll
                {
                    Id = 1,
                    Month = 1,
                    Year = 2025,
                    StaffId = 4,
                    TotalWorkedMinutes = 1,
                    OvertimeMinutes = 0,
                    LateMinutes = 0,
                    EarlyLeaveMinutes = 0,
                    Status = PayrollStatusEnum.Draft,
                    GlobalTaxConfigSnapshot = this.BuildGlobalTaxConfigSnapshot(),
                    RegionalWageSnapshot = this.BuildRegionalWageSnapshot(1),
                    StaffContractSnapshot = new StaffContractSnapshot
                    {
                        HourlyRate = 48,
                        InsuranceSalaryRate = 1
                    },
                    StaffSnapshot = new StaffSnapshot
                    {
                        DependentQuantity = 0,
                        BankCode = "MBBank",
                        AccountNumber = "0762826608"
                    },
                    TaxBracketSnapshot = this.BuildTaxBracketSnapshots(),
                    CreationDate = seedDate
                },
                new Payroll
                {
                    Id = 2,
                    Month = 2,
                    Year = 2025,
                    StaffId = 4,
                    TotalWorkedMinutes = 9600,
                    OvertimeMinutes = 180,
                    LateMinutes = 0,
                    EarlyLeaveMinutes = 0,
                    Status = PayrollStatusEnum.Paid,
                    TransactionId = Guid.Parse("11111111-2222-3333-4444-555555555555"),
                    GlobalTaxConfigSnapshot = this.BuildGlobalTaxConfigSnapshot(),
                    RegionalWageSnapshot = this.BuildRegionalWageSnapshot(1),
                    StaffContractSnapshot = new StaffContractSnapshot
                    {
                        HourlyRate = 55,
                        InsuranceSalaryRate = 1
                    },
                    StaffSnapshot = new StaffSnapshot
                    {
                        DependentQuantity = 0,
                        BankCode = "MBBank",
                        AccountNumber = "0762826608"
                    },
                    SalaryAdvanceSnapshot = new SalaryAdvanceSnapshot
                    {
                        Id = 5,
                        Amount = 140,
                        Reason = "Disbursed advance linked to paid payroll",
                        CreatedBy = 4,
                        CreationDate = seedDate.AddDays(4)
                    },
                    TaxBracketSnapshot = this.BuildTaxBracketSnapshots(),
                    CreationDate = seedDate.AddDays(4)
                }
            };

            var staffIds = this.GetAllStaffIds();
            var nextPayrollId = 3;

            foreach (var staffId in staffIds)
            {
                var marchProfile = this.BuildPayrollProfile(staffId, 3);
                payrolls.Add(new Payroll
                {
                    Id = nextPayrollId,
                    Month = 3,
                    Year = 2025,
                    StaffId = staffId,
                    TotalWorkedMinutes = marchProfile.TotalWorkedMinutes,
                    OvertimeMinutes = marchProfile.OvertimeMinutes,
                    LateMinutes = marchProfile.LateMinutes,
                    EarlyLeaveMinutes = marchProfile.EarlyLeaveMinutes,
                    Status = PayrollStatusEnum.Paid,
                    TransactionId = Guid.Parse($"00000000-0000-0000-0000-{nextPayrollId:000000000000}"),
                    GlobalTaxConfigSnapshot = this.BuildGlobalTaxConfigSnapshot(),
                    RegionalWageSnapshot = this.BuildRegionalWageSnapshot(marchProfile.Region),
                    StaffContractSnapshot = new StaffContractSnapshot
                    {
                        HourlyRate = marchProfile.HourlyRate,
                        InsuranceSalaryRate = 1
                    },
                    StaffSnapshot = new StaffSnapshot
                    {
                        DependentQuantity = marchProfile.DependentQuantity,
                        BankCode = marchProfile.BankCode,
                        AccountNumber = marchProfile.AccountNumber
                    },
                    TaxBracketSnapshot = this.BuildTaxBracketSnapshots(),
                    CreationDate = new DateTime(2025, 3, 31, 0, 0, 0, DateTimeKind.Utc).AddMinutes(staffId)
                });
                nextPayrollId++;

                var aprilProfile = this.BuildPayrollProfile(staffId, 4);
                payrolls.Add(new Payroll
                {
                    Id = nextPayrollId,
                    Month = 4,
                    Year = 2025,
                    StaffId = staffId,
                    TotalWorkedMinutes = aprilProfile.TotalWorkedMinutes,
                    OvertimeMinutes = aprilProfile.OvertimeMinutes,
                    LateMinutes = aprilProfile.LateMinutes,
                    EarlyLeaveMinutes = aprilProfile.EarlyLeaveMinutes,
                    Status = PayrollStatusEnum.Approved,
                    GlobalTaxConfigSnapshot = this.BuildGlobalTaxConfigSnapshot(),
                    RegionalWageSnapshot = this.BuildRegionalWageSnapshot(aprilProfile.Region),
                    StaffContractSnapshot = new StaffContractSnapshot
                    {
                        HourlyRate = aprilProfile.HourlyRate,
                        InsuranceSalaryRate = 1
                    },
                    StaffSnapshot = new StaffSnapshot
                    {
                        DependentQuantity = aprilProfile.DependentQuantity,
                        BankCode = aprilProfile.BankCode,
                        AccountNumber = aprilProfile.AccountNumber
                    },
                    TaxBracketSnapshot = this.BuildTaxBracketSnapshots(),
                    CreationDate = new DateTime(2025, 4, 30, 0, 0, 0, DateTimeKind.Utc).AddMinutes(staffId)
                });
                nextPayrollId++;
            }

            modelBuilder.Entity<Payroll>().HasData(payrolls.ToArray());

            return modelBuilder;
        }

        private List<int> GetAllStaffIds()
        {
            var staffIds = new List<int> { 2, 4, 5, 6, 7, 8, 9, 11, 12, 13 };
            staffIds.AddRange(Enumerable.Range(14, 9));
            return staffIds;
        }

        private (int Region, double HourlyRate, int DependentQuantity, string BankCode, string AccountNumber, int TotalWorkedMinutes, int OvertimeMinutes, int LateMinutes, int EarlyLeaveMinutes) BuildPayrollProfile(int staffId, int month)
        {
            var region = staffId switch
            {
                2 => 2,
                4 => 1,
                5 => 3,
                6 => 1,
                7 => 2,
                8 => 1,
                9 => 2,
                11 => 1,
                12 => 2,
                13 => 1,
                _ => ((staffId - 14) % 3) + 1
            };

            var hourlyRate = staffId switch
            {
                2 => 42,
                4 => 55,
                5 => 18,
                6 => 30,
                7 => 24,
                8 => 27,
                9 => 22,
                11 => 16,
                12 => 17,
                13 => 19,
                _ => 44 + ((staffId - 14) % 5) * 3
            };

            var dependentQuantity = staffId switch
            {
                2 => 0,
                4 => 1,
                5 => 0,
                6 => 2,
                7 => 1,
                8 => 0,
                9 => 0,
                11 => 1,
                12 => 0,
                13 => 2,
                _ => (staffId - 14) % 3
            };

            var bankCode = this.GetBankCodeName(staffId);
            var accountNumber = this.GetAccountNumber(staffId);
            var staffOffset = staffId % 7;
            var monthOffset = month == 3 ? 0 : 1;
            var totalWorkedMinutes = 9600 - staffOffset * 45 - monthOffset * 60;
            var overtimeMinutes = ((staffId + monthOffset) % 4) * 60;
            var lateMinutes = (staffId + monthOffset) % 3 == 0 ? 15 : 0;
            var earlyLeaveMinutes = (staffId + monthOffset) % 5 == 0 ? 10 : 0;

            return (region, hourlyRate, dependentQuantity, bankCode, accountNumber, totalWorkedMinutes, overtimeMinutes, lateMinutes, earlyLeaveMinutes);
        }

        private GlobalTaxConfigSnapshot BuildGlobalTaxConfigSnapshot()
        {
            return new GlobalTaxConfigSnapshot
            {
                PersonalDeductionAmount = 440,
                DependentDeductionAmount = 176,
                ReferenceBaseSalary = 94,
                SocialInsuranceRate = 0.08,
                HealthInsuranceRate = 0.015,
                UnemploymentInsuranceRate = 0.01
            };
        }

        private RegionalWageSnapshot BuildRegionalWageSnapshot(int region)
        {
            return new RegionalWageSnapshot
            {
                Region = region,
                MonthlyWage = region switch
                {
                    1 => 198,
                    2 => 176,
                    _ => 154
                }
            };
        }

        private List<TaxBracketSnapshot> BuildTaxBracketSnapshots()
        {
            return
            [
                new TaxBracketSnapshot { FromAmount = 0, TaxRate = 0.05 },
                new TaxBracketSnapshot { FromAmount = 200, TaxRate = 0.10 },
                new TaxBracketSnapshot { FromAmount = 400, TaxRate = 0.15 },
                new TaxBracketSnapshot { FromAmount = 720, TaxRate = 0.20 },
                new TaxBracketSnapshot { FromAmount = 1280, TaxRate = 0.25 },
                new TaxBracketSnapshot { FromAmount = 2080, TaxRate = 0.30 },
                new TaxBracketSnapshot { FromAmount = 3200, TaxRate = 0.35 }
            ];
        }

        private string GetBankCodeName(int staffId)
        {
            return staffId switch
            {
                2 => "Vietcombank",
                4 => "Techcombank",
                5 => "BIDV",
                6 => "VietinBank",
                7 => "ACB",
                8 => "TPBank",
                9 => "VPBank",
                11 => "Agribank",
                12 => "MSB",
                13 => "OCB",
                _ => this.MapBankCode((staffId - 14) % 25)
            };
        }

        private string GetAccountNumber(int staffId)
        {
            return staffId switch
            {
                2 => "9704360000000002",
                4 => "9704070000000004",
                5 => "9704180000000005",
                6 => "9704150000000006",
                7 => "9704160000000007",
                8 => "9704230000000008",
                9 => "9704320000000009",
                11 => "9704050000000011",
                12 => "9704260000000012",
                13 => "9704480000000013",
                _ => $"9704{staffId - 13:00}{staffId:0000000000}"
            };
        }

        private string MapBankCode(int bankCode)
        {
            return bankCode switch
            {
                0 => "MBBank",
                1 => "Vietcombank",
                2 => "Techcombank",
                3 => "BIDV",
                4 => "VietinBank",
                5 => "ACB",
                6 => "TPBank",
                7 => "VPBank",
                8 => "Agribank",
                9 => "MSB",
                10 => "OCB",
                11 => "KienlongBank",
                12 => "Eximbank",
                13 => "HDBank",
                14 => "Sacombank",
                15 => "VIB",
                16 => "ABBank",
                17 => "LPBank",
                18 => "BacABank",
                19 => "SeABank",
                20 => "SHB",
                21 => "NCB",
                22 => "WooriBank",
                23 => "VietABank",
                24 => "VietBank",
                25 => "NamABank",
                26 => "PGBank",
                _ => "PublicBank"
            };
        }
    }
}
