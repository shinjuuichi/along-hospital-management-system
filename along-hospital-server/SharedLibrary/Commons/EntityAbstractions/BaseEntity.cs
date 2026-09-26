using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SharedLibrary.Commons.EntityAbstractions
{
    public abstract class BaseEntity : Entity
    {
        [Key]
        [Column(Order = 1)]
        public virtual int Id { get; set; }
    }
}
