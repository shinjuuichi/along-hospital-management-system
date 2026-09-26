using SharedLibrary.Commons.EntityAbstractions;
using SharedLibrary.Commons.EntityAnnotations;
namespace BlogSvc.DAL.Models
{
    public class Blog : EntityWithImage
    {
        [MessageRequired]
        [MessageMaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [MessageRequired]
        public string Content { get; set; } = string.Empty;

        [MessageRequired]
        public int BlogCategoryId { get; set; }

        public virtual BlogCategory? BlogCategory { get; set; }
    }
}