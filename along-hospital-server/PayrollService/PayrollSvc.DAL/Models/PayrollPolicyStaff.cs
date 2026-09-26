using Microsoft.EntityFrameworkCore;
using SharedLibrary.Commons.EntityAbstractions;

namespace PayrollSvc.DAL.Models
{
    [PrimaryKey(nameof(PayrollPolicyId), nameof(StaffId))]
    public class PayrollPolicyStaff : Entity
    {
        public int PayrollPolicyId { get; set; }

        public int StaffId { get; set; }

        public virtual PayrollPolicy? PayrollPolicy { get; set; }
    }
}