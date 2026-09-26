using SharedLibrary.Base.Mappers;
using SupplierSvc.DAL.Models;

namespace SupplierSvc.BLL.DTOs.ImportDTOs
{
    public class GetImportDTO : MapFrom<Import>
    {
        public int Id { get; set; }

        public DateTime ImportDate { get; set; }

        public string? Note { get; set; }

        public int ManagerId { get; set; }

        public int SupplierId { get; set; }

        public string? SupplierName { get; set; }

        public List<GetImportDetailDTO> ImportDetails { get; set; } = [];
    }
}