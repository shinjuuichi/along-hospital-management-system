using SharedLibrary.Commons.EntityAbstractions;
using SharedLibrary.Commons.EntityAnnotations;

namespace WorkScheduleSvc.DAL.Models
{
    public class Holiday : BaseEntity
    {
        [MessageRequired]
        public int Day { get; set; }

        [MessageRequired]
        public int Month { get; set; }

        public int? Year { get; set; }

        [MessageRequired]
        [MessageMaxLength(50)]
        public string Name { get; set; } = string.Empty;
    }
}
