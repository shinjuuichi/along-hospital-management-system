using RecruitmentSvc.DAL.Enums;
using RecruitmentSvc.DAL.Models;
using Stateless;

namespace RecruitmentSvc.BLL.StateMachines
{
    public class JobPostingStatusStateMachine
    {
        private readonly JobPosting _jobPosting;
        private readonly StateMachine<JobPostingStatusEnum, JobPostingStatusEnum> _stateMachine;

        public JobPostingStatusStateMachine(JobPosting jobPosting)
        {
            _jobPosting = jobPosting;
            _stateMachine = new StateMachine<JobPostingStatusEnum, JobPostingStatusEnum>(
                () => _jobPosting.Status,
                s => _jobPosting.Status = s);
            Configure();
        }

        private void Configure()
        {
            _stateMachine.Configure(JobPostingStatusEnum.Draft)
                .Permit(JobPostingStatusEnum.Open, JobPostingStatusEnum.Open);

            _stateMachine.Configure(JobPostingStatusEnum.Open)
                .Permit(JobPostingStatusEnum.Closed, JobPostingStatusEnum.Closed);
        }

        public bool CanFire(JobPostingStatusEnum trigger)
        {
            return _stateMachine.CanFire(trigger);
        }

        public void Fire(JobPostingStatusEnum trigger)
        {
            _stateMachine.Fire(trigger);
        }
    }
}
