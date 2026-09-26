using MedicalHistorySvc.DAL.Enums;
using MedicalHistorySvc.DAL.Models;
using Stateless;

namespace MedicalHistorySvc.BLL.StateMachines
{
    public class ComplaintStateMachine
    {
        private readonly Complaint _complaint;
        private readonly StateMachine<ComplaintResolveStatusEnum, ComplaintResolveStatusEnum> _stateMachine;

        public ComplaintStateMachine(Complaint complaint)
        {
            _complaint = complaint;
            _stateMachine = new StateMachine<ComplaintResolveStatusEnum, ComplaintResolveStatusEnum>(
                () => _complaint.ComplaintResolveStatus,
                s => _complaint.ComplaintResolveStatus = s);

            Configure();
        }

        private void Configure()
        {
            _stateMachine.Configure(ComplaintResolveStatusEnum.Pending)
                .Permit(ComplaintResolveStatusEnum.Draft, ComplaintResolveStatusEnum.Draft)
                .Permit(ComplaintResolveStatusEnum.Resolved, ComplaintResolveStatusEnum.Resolved)
                .Permit(ComplaintResolveStatusEnum.Closed, ComplaintResolveStatusEnum.Closed);

            _stateMachine.Configure(ComplaintResolveStatusEnum.Draft)
                .PermitReentry(ComplaintResolveStatusEnum.Draft)
                .Permit(ComplaintResolveStatusEnum.Resolved, ComplaintResolveStatusEnum.Resolved)
                .Permit(ComplaintResolveStatusEnum.Closed, ComplaintResolveStatusEnum.Closed);
        }

        public bool CanFire(ComplaintResolveStatusEnum trigger)
        {
            return _stateMachine.CanFire(trigger);
        }

        public void Fire(ComplaintResolveStatusEnum trigger, string? response = null)
        {
            if (!string.IsNullOrEmpty(response) && (trigger is ComplaintResolveStatusEnum.Draft or ComplaintResolveStatusEnum.Resolved))
            {
                _complaint.Response = response;
            }

            _stateMachine.Fire(trigger);
        }
    }
}
