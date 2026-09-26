using System.ComponentModel.DataAnnotations.Schema;
using SharedLibrary.Commons.EntityAbstractions;
using SharedLibrary.Commons.EntityAnnotations;

namespace PayrollSvc.DAL.Models
{
    public class TaxBracket : AuditEntity
    {
        #region Primary properties
        [NumberHigherThanOrEqualTo(0)]
        public double FromAmount { get; set; }

        [MessageRange(0, 1), Column(TypeName = "numeric(18,4)")]
        public double TaxRate { get; set; }
        #endregion
    }
}
