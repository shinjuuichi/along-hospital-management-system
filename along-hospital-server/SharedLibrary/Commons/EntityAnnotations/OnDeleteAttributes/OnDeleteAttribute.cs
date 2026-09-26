namespace SharedLibrary.Commons.EntityAnnotations.OnDeleteAttributes
{
    /// <summary>
    /// Specifies the delete behavior for a navigation property (both soft delete and hard delete).
    /// Default behavior is <see cref="OnDeleteBehavior.Restrict"/> if not specified.
    /// </summary>
    /// <example>
    /// <code>
    /// // Restrict: Prevent deletion if related records exist (default)
    /// public class Category : AuditEntity
    /// {
    ///     public string Name { get; set; } = null!;
    ///     
    ///     // No attribute = Restrict by default
    ///     public ICollection<Product> Products { get; set; } = [];
    /// }
    /// 
    /// // Cascade: Auto soft-delete related records when parent is deleted
    /// public class Category : AuditEntity
    /// {
    ///     public string Name { get; set; } = null!;
    ///     
    ///     [OnDelete(OnDeleteBehavior.Cascade)]
    ///     public ICollection<Product> Products { get; set; } = [];
    /// }
    /// 
    /// // SetNull: Set FK to null on related records (FK must be nullable!)
    /// public class Category : AuditEntity
    /// {
    ///     public string Name { get; set; } = null!;
    ///     
    ///     [OnDelete(OnDeleteBehavior.SetNull)]
    ///     public ICollection<Product> Products { get; set; } = [];
    /// }
    /// 
    /// // Product with nullable FK for SetNull to work
    /// public class Product : AuditEntity
    /// {
    ///     public string Name { get; set; } = null!;
    ///     public int? CategoryId { get; set; }  // Must be nullable!
    ///     public Category? Category { get; set; }
    /// }
    /// </code>
    /// </example>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class OnDeleteAttribute(OnDeleteBehavior mode) : Attribute
    {
        public OnDeleteBehavior Mode { get; } = mode;
    }
}
