namespace MedicalHistorySvc.BLL.Interfaces
{
    public interface IComplaintPredictionApiService
    {
        Task<string> GetComplaintTypePredictionAsync(string complaintText);
        Task RetrainComplaintTypeModelAsync();
    }
}