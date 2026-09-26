using SharedLibrary.Base.Mappers;
using SupplierSvc.DAL.Models;

namespace SupplierSvc.BLL.DTOs.ImportRequestDTOs
{
    public class GetImportRequestDTO : MapFrom<ImportRequest>
    {
        public int Id { get; set; }

        public DateOnly? RequestDate { get; set; }

        public string? Status { get; set; }

        public int? ApprovedByUserId { get; set; }

        public List<GetImportRequestDetailDTO> Details { get; set; } = [];
    }
  }