using AutoMapper;
using InpatientResourceSvc.BLL.DTOs.BedCategoryDTOs;
using InpatientResourceSvc.BLL.Interfaces;
using InpatientResourceSvc.DAL.Models;
using MessageBroker.Contracts.MedicalServiceContracts;
using MessageBroker.Events.MedicalServiceEvents;
using SharedLibrary.Base.Data;
using SharedLibrary.Base.MessageBuses;
using SharedLibrary.Base.Services;

namespace InpatientResourceSvc.BLL.Implements
{
    public class BedCategoryService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IMessageBus messageBus)
        : BaseService<BedCategory, CreateBedCategoryDTO, UpdateBedCategoryDTO, GetBedCategoryDTO>(unitOfWork, mapper),
            IBedCategoryService
    {
        private readonly IMessageBus _messageBus = messageBus;

        public override async Task<GetBedCategoryDTO> CreateAsync(CreateBedCategoryDTO createDTO)
        {
            var medicalService = await _messageBus.RequestAsync<GetMedicalServiceByCodeEvent, GetMedicalServiceContract>(
                new GetMedicalServiceByCodeEvent { Code = createDTO.Code });

            createDTO.Code = medicalService.Code;

            var bedCategory = _mapper.Map<BedCategory>(createDTO);

            var result = await _repository.AddAsync(bedCategory);
            await _unitOfWork.SaveChangeAsync();

            return await this.GetByIdAsync(result.Id);
        }
    }
}
