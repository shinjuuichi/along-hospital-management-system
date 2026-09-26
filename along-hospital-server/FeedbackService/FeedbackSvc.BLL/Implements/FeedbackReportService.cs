using AutoMapper;
using FeedbackSvc.BLL.DTOs.FeedbackReportDTOs;
using FeedbackSvc.BLL.Interfaces;
using FeedbackSvc.BLL.StateMachines;
using FeedbackSvc.DAL.Enums;
using FeedbackSvc.DAL.Models;
using SharedLibrary.Base.Data;
using SharedLibrary.Base.Data.SqlServerDb;
using SharedLibrary.Base.Services;
using SharedLibrary.Commons.Exceptions;

namespace FeedbackSvc.BLL.Implements
{
    public class FeedbackReportService(
        IUnitOfWork unitOfWork,
        IMapper mapper)
        : BaseService<FeedbackReport, CreateFeedbackReportDTO, NoUpdateFeedbackReportDTO, GetFeedbackReportDTO>(
            unitOfWork,
            mapper, includes: [nameof(Feedback)]),
        IFeedbackReportService
    {
        private readonly IGenericRepository<Feedback> _feedbackRepository = unitOfWork.Repository<Feedback>();

        public override async Task<GetFeedbackReportDTO> CreateAsync(CreateFeedbackReportDTO createFeedbackReportDTO)
        {
            await this.EnsureFeedbackExistsAsync(createFeedbackReportDTO.FeedbackId);

            return await base.CreateAsync(createFeedbackReportDTO);
        }

        public async Task UpdateStatusAsync(int id, FeedbackReportStatusEnum feedbackReportStatus)
        {
            var feedbackReport = await _repository.GetByIdAsync(id)
                ?? throw new DataNotFoundException(typeof(FeedbackReport), id);

            var feedbackReportStateMachine = new FeedbackReportStateMachine(feedbackReport);
            if (!feedbackReportStateMachine.CanFire(feedbackReportStatus))
            {
                throw new InvalidDataException($"Cannot change status from {feedbackReport.Status} to {feedbackReportStatus}");
            }

            if (feedbackReportStatus == FeedbackReportStatusEnum.Resolved)
            {
                var feedback = await this.EnsureFeedbackExistsAsync(feedbackReport.FeedbackId);
                feedback.FeedbackStatus = FeedbackStatusEnum.Hidden;
                _feedbackRepository.Update(feedback);
            }

            feedbackReportStateMachine.Fire(feedbackReportStatus);
            _repository.Update(feedbackReport);
            await _unitOfWork.SaveChangeAsync();
        }

        private async Task<Feedback> EnsureFeedbackExistsAsync(int feedbackId)
        {
            return await _feedbackRepository.GetByIdAsync(feedbackId)
                ?? throw new DataNotFoundException(typeof(Feedback), feedbackId);
        }
    }
}