using MedicineSvc.DAL.Models;
using SharedLibrary.Base.Mappers;

namespace MedicineSvc.BLL.DTOs.OptionDTOs
{
    public class CreateOptionDTO : MapTo<Option>
    {
        public string? OptionName { get; set; }
    }
}
