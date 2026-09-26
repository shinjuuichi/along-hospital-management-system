using MedicineSvc.BLL.DTOs.MedicineDTOs;
using MedicineSvc.DAL.Models;
using SharedLibrary.Base.Mappers;

namespace MedicineSvc.BLL.DTOs.OptionDTOs
{
    public class GetOptionDTO : MapFrom<Option>
    {
        public int Id { get; set; }

        public string? OptionName { get; set; }

        public List<GetMedicineDTO> Medicines { get; set; } = [];

        public List<GetOptionValueDTO> OptionValues { get; set; } = [];
    }
}
