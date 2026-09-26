namespace FeedbackSvc.BLL.Interfaces.MachineLearning
{
    public interface IFeedbackPredictionApiService
    {
        Task<bool> GetFeedbackToxicPredictionAsync(string feedbackContent);
        Task<string> GetFeedbackTypePredictionAsync(string feedbackContent);
        Task RetrainFeedbackTypeModelAsync();
        Task RetrainFeedbackToxicModelAsync();
    }
}