using MedicineSvc.DAL.Models;
using SharedLibrary.Base.Mappers;

namespace MedicineSvc.BLL.DTOs.OptionDTOs
{
    public class UpdateOptionValueDTO : MapTo<OptionValue>
    {
        public string? ValueName { get; set; }

        public int UnitMultiplier { get; set; }

        public bool IsActive { get; set; }
    }
}
