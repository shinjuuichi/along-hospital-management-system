using SharedLibrary.Commons.EntityAbstractions;
using SharedLibrary.Commons.EntityAnnotations;

namespace MedicalHistorySvc.DAL.Models
{
    public class ComplaintSummary : BaseEntity
    {
        public int Year { get; set; }

        public int WeekOfYear { get; set; }

        [MessageRequired, MessageMaxLength(2000)]
        public string Summary { get; set; } = string.Empty;
    }
}
