using AutoMapper;
using FeedbackSvc.BLL.DTOs.FeedbackRespondDTOs;
using FeedbackSvc.BLL.Interfaces;
using FeedbackSvc.DAL.Enums;
using FeedbackSvc.DAL.Models;
using MessageBroker.Events.FeedbackEvents;
using MessageBroker.Events.SendEmailEvents;
using SharedLibrary.Base.Data;
using SharedLibrary.Base.MessageBuses;
using SharedLibrary.Base.Services;
using SharedLibrary.Enums;
using SharedLibrary.Services.Interfaces;

namespace FeedbackSvc.BLL.Implements
{
    public class FeedbackRespondService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ICurrentUserService currentUserService,
        IMessageBus messageBus,
        IFeedbackService feedbackService)
        : BaseService<FeedbackRespond, CreateFeedbackRespondDTO, UpdateFeedbackRespondDTO, GetFeedbackRespondDTO>(
            unitOfWork,
            mapper), IFeedbackRespondService
    {
        private readonly ICurrentUserService _currentUserService = currentUserService;
        private readonly IMessageBus _messageBus = messageBus;
        private readonly IFeedbackService _feedbackService = feedbackService;

        public override async Task<GetFeedbackRespondDTO> CreateAsync(CreateFeedbackRespondDTO createFeedbackRespondDTO)
        {
            var feedback = await _feedbackService.GetByIdAsync(createFeedbackRespondDTO.FeedbackId);
            var getFeedbackRespondDTO = await base.CreateAsync(createFeedbackRespondDTO);

            if (_currentUserService.Role == RoleEnum.Patient)
            {
                await _feedbackService.UpdateFeedbackReplyStatusAsync(feedback.Id, nameof(FeedbackReplyStatusEnum.WaitingStaff));
            }
            else
            {
                await _feedbackService.UpdateFeedbackReplyStatusAsync(feedback.Id, nameof(FeedbackReplyStatusEnum.Replied));
            }

            var sendFeedbackRespondEvent = _mapper.Map<SendFeedbackRespondEvent>(getFeedbackRespondDTO);
            await _messageBus.PublishAsync(sendFeedbackRespondEvent);

            var predictFeedbackRespondToxicEvent = _mapper.Map<PredictFeedbackRespondToxicEvent>(getFeedbackRespondDTO);
            await _messageBus.PublishAsync(predictFeedbackRespondToxicEvent);

            return getFeedbackRespondDTO;
        }

        public override async Task<GetFeedbackRespondDTO> UpdateAsync(int id, UpdateFeedbackRespondDTO updateFeedbackRespondDTO)
        {
            var getFeedbackRespondDTO = await base.UpdateAsync(id, updateFeedbackRespondDTO);

            var predictFeedbackRespondToxicEvent = _mapper.Map<PredictFeedbackRespondToxicEvent>(getFeedbackRespondDTO);
            await _messageBus.PublishAsync(predictFeedbackRespondToxicEvent);

            return getFeedbackRespondDTO;
        }

        public async Task BanFeedbackRespondAsync(int id)
        {
            var feedbackRespond = await _repository.GetByIdAsync(id);
            if (feedbackRespond != null)
            {
                feedbackRespond.FeedbackStatus = FeedbackRespondStatusEnum.Hidden;
                _repository.Update(feedbackRespond);
                await _unitOfWork.SaveChangeAsync();
            }
        }
    }
}
