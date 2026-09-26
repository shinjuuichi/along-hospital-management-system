using Stateless;
using SupplierSvc.DAL.Enums;
using SupplierSvc.DAL.Models;

namespace SupplierSvc.BLL.StateMachines
{
    public class ImportRequestStatusStateMachine
    {
        private readonly StateMachine<ImportRequestStatusEnum, ImportRequestStatusEnum> _stateMachine;
        private readonly ImportRequest _importRequest;
        private readonly int _userId;

        public ImportRequestStatusStateMachine(ImportRequest importRequest, int userId)
        {
            _importRequest = importRequest;
            _userId = userId;

            _stateMachine = new StateMachine<ImportRequestStatusEnum, ImportRequestStatusEnum>(
                () => _importRequest.Status,
                s => _importRequest.Status = s);

            Configure();
        }

        private void Configure()
        {
            _stateMachine.Configure(ImportRequestStatusEnum.Pending)
                .Permit(ImportRequestStatusEnum.Approved, ImportRequestStatusEnum.Approved)
                .Permit(ImportRequestStatusEnum.Rejected, ImportRequestStatusEnum.Rejected)
                .Permit(ImportRequestStatusEnum.Cancelled, ImportRequestStatusEnum.Cancelled);

            _stateMachine.Configure(ImportRequestStatusEnum.Approved)
                .Permit(ImportRequestStatusEnum.Created, ImportRequestStatusEnum.Created)
                .OnEntry(() => _importRequest.ApprovedByUserId = _userId);

            _stateMachine.Configure(ImportRequestStatusEnum.Created);
        }

        public bool CanFire(ImportRequestStatusEnum trigger)
        {
            return _stateMachine.CanFire(trigger);
        }

        public void Fire(ImportRequestStatusEnum trigger)
        {
            _stateMachine.Fire(trigger);
        }
    }
}