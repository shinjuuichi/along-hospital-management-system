using SharedLibrary.Commons.EntityAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SharedLibrary.Commons.EntityAbstractions
{
    public abstract class EntityWithMultiImages : AuditEntity
    {
        [MessageMaxLength(255)]
        [Column(Order = 2)]
        public string[] Images { get; set; } = [];
    }
}
