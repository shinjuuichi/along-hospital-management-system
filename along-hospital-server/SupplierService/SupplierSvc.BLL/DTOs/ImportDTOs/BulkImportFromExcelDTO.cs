using Microsoft.AspNetCore.Http;
using SharedLibrary.Commons.EntityAnnotations;

namespace SupplierSvc.BLL.DTOs.ImportDTOs
{
    public class BulkImportFromExcelDTO
    {
        [MessageRequiredFile]
        public IFormFile File { get; set; } = null!;
    }
}