using MessageBroker.Contracts.AppointmentContracts;
using MessageBroker.Contracts.AuthAccountContracts.GetUserDataContracts;
using MessageBroker.Contracts.InPatientResourceContracts;
using MessageBroker.Contracts.MedicalHistoryContracts;
using MessageBroker.Contracts.StaffContracts.SpecialtyContracts;

namespace QueueSvc.BLL.Interfaces
{
    public interface IQueueMessageBusService
    {
        Task<GetMedicalHistoryContract> GetMedicalHistoryAndValidateStatusAsync(int medicalHistoryId, string? status = null);
        Task<List<GetMedicalHistoryContract>> GetAllMedicalHistoryContractsByIdsAsync(List<int> medicalHistoryIds);
        Task AssignDoctorToMedicalHistoryAsync(int medicalHistoryId, int doctorId);

        Task<List<GetAppointmentContract>> GetAllAppointmentContractsByDateAsync(DateOnly date);
        Task UpdateListAppointmentToCompletedAsync(List<int> appointmentIds);

        Task<GetSpecialtyByIdContract> GetSpecialtyContractAsync(int specialtyId);
        Task<List<GetSpecialtyByIdContract>> GetAllSpecialtyContractsByIdsAsync(List<int> specialtyIds);

        Task<List<(int RoomId, int? DoctorId)>> GetListTodayWorkingMedicalRoomAsync();
        Task<int?> GetTodayWorkingDoctorByRoomIdAsync(int roomId);
        Task<(GetRoomContract Room, GetStaffDataByUserIdContract? Doctor)> GetRoomAndDoctorContractByRoomIdAsync(int roomId);
        Task ValidateStaffActionAsync(int? roomId = null);

        Task<List<GetRoomContract>> GetAllRoomContractsByIdsAsync(List<int> roomIds);
        Task<List<GetStaffDataByUserIdContract>> GetAllDoctorContractsByIdsAsync(List<int> doctorIds);
        Task<List<GetPatientDataByUserIdContract>> GetAllPatientContractsByIdsAsync(List<int> patientIds);
    }
}