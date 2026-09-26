namespace SupplierSvc.BLL.DTOs.ImportExcelDTOs
{
    public class ReadImportRowFromExcelDTO
    {
        public string? SupplierName { get; set; }
        public string? MedicineName { get; set; }
        public int Quantity { get; set; }
        public double UnitPrice { get; set; }
        public string? Note { get; set; }
    }
}