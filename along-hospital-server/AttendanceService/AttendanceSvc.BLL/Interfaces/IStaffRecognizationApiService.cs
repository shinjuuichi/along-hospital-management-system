using Microsoft.AspNetCore.Http;

namespace AttendanceSvc.BLL.Interfaces
{
    public interface IStaffRecognizationApiService
    {
        Task<int> RecognizeStaffAsync(IFormFile? file);
        Task<bool> CheckIdentificationExistAsync(int staffId);
        Task<string> EnrollAsync(int staffId, List<IFormFile> images);
        Task<string> ResetIdentificationAsync(int staffId);
    }
}