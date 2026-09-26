using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Base.Controllers;
using SharedLibrary.Commons.Filters;
using SharedLibrary.Commons.Results;
using SharedLibrary.Enums;
using SupplierSvc.BLL.DTOs.ImportDTOs;
using SupplierSvc.BLL.Interfaces;

namespace SupplierSvc.WebAPI.Controllers
{
    [Authorize(Roles = nameof(RoleEnum.InventoryClerk))]
    public class ImportManagementController(
        IImportService importService,
        IImportExcelService importExcelService)
        : GetController<GetImportDTO, FilterDTO>(importService)
    {
        private readonly IImportService _importService = importService;
        //private readonly IImportExcelService _importExcelService = importExcelService;
        private const string EntityName = "Import";

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateImportDTO updateDTO)
        {
            var result = await _importService.UpdateAsync(id, updateDTO);
            return Result.SuccessData(result, $"{EntityName} updated successfully");
        }

        [HttpPost("from-approved-request")]
        public async Task<IActionResult> CreateFromApprovedRequest(CreateImportDTO createDTO)
        {
            var result = await _importService.CreateImportFromApprovedAsync(createDTO);
            return Result.SuccessData(result, $"Import created successfully from ImportRequest #{createDTO.ImportRequestId}");
        }

        //[HttpPost("bulk-import-from-excel")]
        //[RequestSizeLimit(10 * 1024 * 1024)]
        //[RequestFormLimits(MultipartBodyLengthLimit = 10 * 1024 * 1024)]
        //public async Task<IActionResult> BulkImportFromExcel([FromForm] BulkImportFromExcelDTO dto)
        //{
        //    if (dto.File == null || dto.File.Length == 0)
        //    {
        //        return Result.FailError("No file provided or file is empty", "File validation failed", 400);
        //    }

        //    var excelRows = await _importExcelService.ReadImportRowsFromExcelAsync(dto.File.OpenReadStream(), dto.File.FileName);

        //    if (excelRows.Count == 0)
        //    {
        //        return Result.FailError("Excel file has no valid data", "Validation failed", 400);
        //    }

        //    var result = await importService.BulkImportFromExcelAsync(excelRows);
        //    return Result.SuccessData(result, "Bulk import completed successfully");
        //}
    }
}
