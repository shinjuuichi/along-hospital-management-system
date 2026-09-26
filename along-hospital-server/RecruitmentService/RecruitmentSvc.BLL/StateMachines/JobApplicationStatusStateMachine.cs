using RecruitmentSvc.DAL.Enums;
using RecruitmentSvc.DAL.Models;
using Stateless;

namespace RecruitmentSvc.BLL.StateMachines
{
    public class JobApplicationStatusStateMachine
    {
        private readonly JobApplication _jobApplication;
        private readonly StateMachine<JobApplicationStatusEnum, JobApplicationStatusEnum> _stateMachine;

        public JobApplicationStatusStateMachine(JobApplication jobApplication)
        {
            _jobApplication = jobApplication;
            _stateMachine = new StateMachine<JobApplicationStatusEnum, JobApplicationStatusEnum>(
                () => _jobApplication.ApplicationStatus,
                s => _jobApplication.ApplicationStatus = s);
            Configure();
        }

        private void Configure()
        {
            _stateMachine.Configure(JobApplicationStatusEnum.Applied)
                .Permit(JobApplicationStatusEnum.Interviewing, JobApplicationStatusEnum.Interviewing)
                .Permit(JobApplicationStatusEnum.Failed, JobApplicationStatusEnum.Failed);

            _stateMachine.Configure(JobApplicationStatusEnum.Interviewing)
                .Permit(JobApplicationStatusEnum.Passed, JobApplicationStatusEnum.Passed)
                .Permit(JobApplicationStatusEnum.Failed, JobApplicationStatusEnum.Failed);

            _stateMachine.Configure(JobApplicationStatusEnum.Passed)
                .Ignore(JobApplicationStatusEnum.Passed);

            _stateMachine.Configure(JobApplicationStatusEnum.Failed)
                .Ignore(JobApplicationStatusEnum.Failed);
        }

        public bool CanFire(JobApplicationStatusEnum trigger)
        {
            return _stateMachine.CanFire(trigger);
        }

        public void Fire(JobApplicationStatusEnum trigger)
        {
            _stateMachine.Fire(trigger);
        }
    }
}