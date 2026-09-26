using SharedLibrary.Services.Interfaces;
using SupplierSvc.BLL.DTOs.ImportExcelDTOs;
using SupplierSvc.BLL.Interfaces;

namespace SupplierSvc.BLL.Implements
{
    public class ImportExcelService : IImportExcelService
    {
        private readonly IExcelService _excelService;

        public ImportExcelService(IExcelService excelService) => _excelService = excelService;

        public async Task<List<ReadImportRowFromExcelDTO>> ReadImportRowsFromExcelAsync(
            Stream fileStream,
            string fileName)
        {
            var dictionaries = await _excelService.ReadExcelToDictionaryAsync(fileStream, fileName);
            var result = new List<ReadImportRowFromExcelDTO>();

            foreach (var dict in dictionaries)
            {
                var row = new ReadImportRowFromExcelDTO();

                // Map columns (case-insensitive)
                var supplierNameKey = dict.Keys.FirstOrDefault(k =>
                    k.Equals("SupplierName", StringComparison.OrdinalIgnoreCase) ||
                    k.Equals("Supplier", StringComparison.OrdinalIgnoreCase));
                if (supplierNameKey != null && dict.TryGetValue(supplierNameKey, out var supplierNameValue))
                {
                    row.SupplierName = supplierNameValue?.ToString()?.Trim();
                }

                var medicineNameKey = dict.Keys.FirstOrDefault(k =>
                    k.Equals("MedicineName", StringComparison.OrdinalIgnoreCase) ||
                    k.Equals("Medicine", StringComparison.OrdinalIgnoreCase));
                if (medicineNameKey != null && dict.TryGetValue(medicineNameKey, out var medicineNameValue))
                {
                    row.MedicineName = medicineNameValue?.ToString()?.Trim();
                }

                var quantityKey = dict.Keys.FirstOrDefault(k =>
                    k.Equals("Quantity", StringComparison.OrdinalIgnoreCase));
                if (quantityKey != null && dict.TryGetValue(quantityKey, out var quantityValue) && int.TryParse(quantityValue?.ToString(), out var quantity))
                {
                    row.Quantity = quantity;
                }

                var unitPriceKey = dict.Keys.FirstOrDefault(k =>
                    k.Equals("UnitPrice", StringComparison.OrdinalIgnoreCase) ||
                    k.Equals("Price", StringComparison.OrdinalIgnoreCase));
                if (unitPriceKey != null && dict.TryGetValue(unitPriceKey, out var unitPriceValue) && double.TryParse(unitPriceValue?.ToString(), out var unitPrice))
                {
                    row.UnitPrice = unitPrice;
                }

                var noteKey = dict.Keys.FirstOrDefault(k =>
                    k.Equals("Note", StringComparison.OrdinalIgnoreCase) ||
                    k.Equals("Notes", StringComparison.OrdinalIgnoreCase));
                if (noteKey != null && dict.TryGetValue(noteKey, out var noteValue))
                {
                    row.Note = noteValue?.ToString()?.Trim();
                }

                result.Add(row);
            }

            return result;
        }
    }
}