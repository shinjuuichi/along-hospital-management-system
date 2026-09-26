using AutoMapper;
using MedicineSvc.BLL.DTOs.OptionDTOs;
using MedicineSvc.BLL.Interfaces;
using MedicineSvc.DAL.Models;
using SharedLibrary.Base.Data;
using SharedLibrary.Base.Services;

namespace MedicineSvc.BLL.Implements
{
    public class OptionValueService(
        IUnitOfWork unitOfWork,
        IMapper mapper)
        : BaseService<OptionValue, CreateOptionValueDTO, UpdateOptionValueDTO, GetOptionValueDTO>(
            unitOfWork,
            mapper,
            includes: [nameof(OptionValue.Option)]),
            IOptionValueService;
}
