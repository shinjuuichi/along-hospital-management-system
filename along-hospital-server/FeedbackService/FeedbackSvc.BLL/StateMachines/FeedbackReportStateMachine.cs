using FeedbackSvc.DAL.Enums;
using FeedbackSvc.DAL.Models;
using Stateless;

namespace FeedbackSvc.BLL.StateMachines
{
    public class FeedbackReportStateMachine
    {
        private readonly FeedbackReport _feedbackReport;
        private readonly StateMachine<FeedbackReportStatusEnum, FeedbackReportStatusEnum> _stateMachine;

        public FeedbackReportStateMachine(FeedbackReport feedbackReport)
        {
            _feedbackReport = feedbackReport;
            _stateMachine = new StateMachine<FeedbackReportStatusEnum, FeedbackReportStatusEnum>(
                () => _feedbackReport.Status,
                status => _feedbackReport.Status = status);

            Configure();
        }

        private void Configure()
        {
            _stateMachine.Configure(FeedbackReportStatusEnum.Pending)
                .Permit(FeedbackReportStatusEnum.Resolved, FeedbackReportStatusEnum.Resolved)
                .Permit(FeedbackReportStatusEnum.Rejected, FeedbackReportStatusEnum.Rejected);
        }

        public bool CanFire(FeedbackReportStatusEnum trigger) => _stateMachine.CanFire(trigger);

        public void Fire(FeedbackReportStatusEnum trigger) => _stateMachine.Fire(trigger);
    }
}