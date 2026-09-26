using RecruitmentSvc.DAL.Enums;
using RecruitmentSvc.DAL.Models;
using Stateless;

namespace RecruitmentSvc.BLL.StateMachines
{
    public class InterviewResultStateMachine
    {
        private readonly Interview _interview;
        private readonly StateMachine<InterviewResultEnum, InterviewResultEnum> _stateMachine;

        public InterviewResultStateMachine(Interview interview)
        {
            _interview = interview;
            _stateMachine = new StateMachine<InterviewResultEnum, InterviewResultEnum>(
                () => _interview.Result,
                s => _interview.Result = s);
            Configure();
        }

        private void Configure()
        {
            _stateMachine.Configure(InterviewResultEnum.Pending)
                .Permit(InterviewResultEnum.Passed, InterviewResultEnum.Passed)
                .Permit(InterviewResultEnum.Failed, InterviewResultEnum.Failed)
                .Permit(InterviewResultEnum.Cancelled, InterviewResultEnum.Cancelled);

        }

        public bool CanFire(InterviewResultEnum trigger)
        {
            return _stateMachine.CanFire(trigger);
        }

        public void Fire(InterviewResultEnum trigger)
        {
            _stateMachine.Fire(trigger);
        }

        public JobApplicationStatusEnum? GetTargetJobApplicationStatus(InterviewResultEnum result)
        {
            return result switch
            {
                InterviewResultEnum.Passed => JobApplicationStatusEnum.Passed,
                InterviewResultEnum.Failed => JobApplicationStatusEnum.Failed,
                InterviewResultEnum.Cancelled => JobApplicationStatusEnum.Failed,
                _ => null
            };
        }
    }
}
