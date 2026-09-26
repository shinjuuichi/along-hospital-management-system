using MedicineSvc.DAL.Models;
using SharedLibrary.Base.Mappers;

namespace MedicineSvc.BLL.DTOs.OptionDTOs
{
    public class CreateOptionValueDTO : MapTo<OptionValue>
    {
        public string? ValueName { get; set; }

        public int OptionId { get; set; }

        public int UnitMultiplier { get; set; }
    }
}
