using FeedbackSvc.BLL.DTOs.FeedbackDTOs;
using SharedLibrary.Base.Services;

namespace FeedbackSvc.BLL.Interfaces.Management
{
    public interface IFeedbackManagementService : IBaseGetService<GetFeedbackAndMedicineDTO>;
}