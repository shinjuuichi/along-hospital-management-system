using SupplierSvc.BLL.DTOs.ImportDTOs;

namespace SupplierSvc.BLL.DTOs.ImportExcelDTOs
{
    public class BulkImportFromExcelResponseDTO
    {
        public int TotalRowsProcessed { get; set; }
        public int SuppliersCreated { get; set; }
        public int MedicinesCreated { get; set; }
        public int ImportsCreated { get; set; }
        public List<GetImportDTO> CreatedImports { get; set; } = [];
    }
}