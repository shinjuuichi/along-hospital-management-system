using System.ComponentModel.DataAnnotations;

namespace SharedLibrary.Commons.Filters
{
    public enum FilterOperationEnum
    {
        [Display(Name = "=")]
        Equal,

        [Display(Name = "!=")]
        NotEqual,

        [Display(Name = ">")]
        GreaterThan,

        [Display(Name = "<")]
        LessThan,

        [Display(Name = ">=")]
        GreaterThanOrEqual,

        [Display(Name = "<=")]
        LessThanOrEqual,

        [Display(Name = "=*")]
        Contains,

        [Display(Name = "!*")]
        NotContains,

        [Display(Name = "^")]
        StartsWith,

        [Display(Name = "!^")]
        NotStartsWith,

        [Display(Name = "$")]
        EndsWith,

        [Display(Name = "!$")]
        NotEndsWith,
    }
}
