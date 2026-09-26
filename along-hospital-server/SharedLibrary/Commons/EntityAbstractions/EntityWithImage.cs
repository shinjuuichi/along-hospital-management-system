using SharedLibrary.Commons.EntityAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SharedLibrary.Commons.EntityAbstractions
{
    public abstract class EntityWithImage : AuditEntity
    {
        [MessageMaxLength(255)]
        [Column(Order = 2)]
        public string? Image { get; set; }
    }
}
