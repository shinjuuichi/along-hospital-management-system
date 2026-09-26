using Microsoft.AspNetCore.Http;
using SharedLibrary.Enums;
using SharedLibrary.Utils;
using System.ComponentModel.DataAnnotations;

namespace SharedLibrary.Commons.EntityAnnotations
{
    /// Usage examples:
    /// <code>
    /// // No annotation on property -> default is Image only
    /// public IFormFile? Avatar { get; set; }
    ///
    /// // Default case (same as FileType.Image)
    /// [AllowFileType]
    /// public IFormFile? Avatar { get; set; }
    ///
    /// // Single type: Image files only
    /// [AllowFileType(FileType.Image)]
    /// public IFormFile? Avatar { get; set; }
    ///
    /// // Single type: Document files only
    /// [AllowFileType(FileType.Document)]
    /// public IFormFile? Attachment { get; set; }
    ///
    /// // Multiple types: Image OR Document
    /// [AllowFileType(FileType.Image, FileType.Document)]
    /// public IFormFile? Attachment { get; set; }
    ///
    /// // Multiple types: Image OR Document OR Video
    /// [AllowFileType(FileType.Image, FileType.Document, FileType.Video)]
    /// public IFormFile? MediaFile { get; set; }
    ///
    /// // All built-in supported file types
    /// [AllowFileType(FileType.Other)]
    /// public IFormFile? AnySupportedFile { get; set; }
    ///
    /// // Custom extensions only
    /// [AllowFileType(FileType.Custom, CustomExtensions = new[] { ".pdf", ".docx" })]
    /// public IFormFile? CustomFile { get; set; }
    ///
    /// // Multiple types + custom extensions
    /// [AllowFileType(FileType.Image, FileType.Custom, CustomExtensions = new[] { ".svg" })]
    /// public IFormFile? FlexibleFile { get; set; }
    /// </code>
    /// </remarks>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class AllowFileTypeAttribute(params FileType[] fileTypes) : ValidationAttribute
    {
        public FileType[] FileTypes { get; } = fileTypes is { Length: > 0 } ? fileTypes : [FileType.Image];

        public string[] CustomExtensions { get; set; } = [];

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is null)
            {
                return ValidationResult.Success;
            }

            var allowedExtensions = FileExtensionMap.GetAllowedExtensions(FileTypes, CustomExtensions);
            var memberName = validationContext.MemberName ?? validationContext.DisplayName;

            if (value is IFormFile file)
            {
                return ValidateFile(file, allowedExtensions, memberName);
            }

            if (value is IEnumerable<IFormFile> files)
            {
                foreach (var f in files)
                {
                    var result = ValidateFile(f, allowedExtensions, memberName);
                    if (result != ValidationResult.Success)
                    {
                        return result;
                    }
                }
            }

            return ValidationResult.Success;
        }

        private static ValidationResult? ValidateFile(IFormFile file, HashSet<string> allowedExtensions, string memberName)
        {
            var extension = Path.GetExtension(file.FileName);

            if (string.IsNullOrEmpty(extension) || !allowedExtensions.Contains(extension))
            {
                var allowed = string.Join(", ", allowedExtensions.Order());
                return new ValidationResult(
                    $"File '{file.FileName}' has an unsupported extension. Allowed: {allowed}",
                    [memberName]);
            }

            return ValidationResult.Success;
        }
    }
}
