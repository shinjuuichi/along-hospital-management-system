namespace SharedLibrary.Commons.EntityAbstractions
{
    public abstract class AuditEntity : BaseEntity
    {
        public DateTime CreationDate { get; set; } = DateTime.UtcNow;

        public int? CreatedBy { get; set; }

        public DateTime? ModificationDate { get; set; }

        public int? ModifiedBy { get; set; }

        public DateTime? DeletionDate { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}
