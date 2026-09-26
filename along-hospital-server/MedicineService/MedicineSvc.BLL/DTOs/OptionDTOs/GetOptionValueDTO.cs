using MedicineSvc.DAL.Models;
using SharedLibrary.Base.Mappers;

namespace MedicineSvc.BLL.DTOs.OptionDTOs
{
    public class GetOptionValueDTO : MapFrom<OptionValue>
    {
        public int Id { get; set; }

        public string? ValueName { get; set; }

        public int UnitMultiplier { get; set; }

        public bool IsActive { get; set; }

        public int OptionId { get; set; }

        public GetOptionDTO? Option { get; set; }
    }
}
