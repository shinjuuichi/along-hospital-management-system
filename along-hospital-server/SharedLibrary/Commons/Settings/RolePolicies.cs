using SharedLibrary.Enums;

namespace SharedLibrary.Commons.Settings
{
    public static class RolePolicies
    {
        public const string TeleHealthSessionCallRolePolicy =
            $"{nameof(RoleEnum.Doctor)}," +
            $"{nameof(RoleEnum.Patient)}";

        public const string FeedbackRespondRolePolicy =
            $"{nameof(RoleEnum.HotlineAgent)}," +
            $"{nameof(RoleEnum.Patient)}";

        public const string StaffRolePolicy =
            $"{nameof(RoleEnum.Manager)}," +
            $"{nameof(RoleEnum.Doctor)}," +
            $"{nameof(RoleEnum.Nurse)}," +
            $"{nameof(RoleEnum.HR)}," +
            $"{nameof(RoleEnum.Pharmacist)}," +
            $"{nameof(RoleEnum.Accountant)}," +
            $"{nameof(RoleEnum.Marketer)}," +
            $"{nameof(RoleEnum.Receptionist)}," +
            $"{nameof(RoleEnum.HotlineAgent)}," +
            $"{nameof(RoleEnum.InventoryClerk)}";

        public const string CustomerSupportRolePolicy =
            $"{nameof(RoleEnum.HotlineAgent)}," +
            $"{nameof(RoleEnum.Manager)}";

        public const string MedicalStaffRolePolicy =
            $"{nameof(RoleEnum.Doctor)}," +
            $"{nameof(RoleEnum.Nurse)}," +
            $"{nameof(RoleEnum.Accountant)}," +
            $"{nameof(RoleEnum.Receptionist)}," +
            $"{nameof(RoleEnum.Manager)}";

        public const string StaffRequestRolePolicy =
            $"{nameof(RoleEnum.Manager)}," +
            $"{nameof(RoleEnum.HR)}";

        public const string SalaryAdvanceManagementRolePolicy =
            $"{nameof(RoleEnum.Manager)}," +
            $"{nameof(RoleEnum.Accountant)}";

        public const string QueueManagementRolePolicy =
            $"{nameof(RoleEnum.Nurse)}," +
            $"{nameof(RoleEnum.Receptionist)}," +
            $"{nameof(RoleEnum.Doctor)}";

        public const string PayrollManagementRolePolicy =
            $"{nameof(RoleEnum.Accountant)}," +
            $"{nameof(RoleEnum.Manager)}";
    }
}
