using AutoMapper;
using MedicineSvc.BLL.DTOs.OptionDTOs;
using MedicineSvc.BLL.Interfaces;
using MedicineSvc.DAL.Models;
using SharedLibrary.Base.Data;
using SharedLibrary.Base.Services;

namespace MedicineSvc.BLL.Implements
{
    public class OptionService(
        IUnitOfWork unitOfWork,
        IMapper mapper)
        : BaseService<Option, CreateOptionDTO, UpdateOptionDTO, GetOptionDTO>(
            unitOfWork,
            mapper,
            includes: [
                $"{nameof(Option.MedicineUnitOptions)}.{nameof(MedicineUnitOption.MedicineUnit)}",
                nameof(Option.OptionValues)
            ]),
            IOptionService;
}
