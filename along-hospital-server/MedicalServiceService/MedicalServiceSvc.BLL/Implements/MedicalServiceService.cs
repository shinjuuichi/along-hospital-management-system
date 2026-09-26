using AutoMapper;
using MedicalServiceSvc.BLL.DTOs;
using MedicalServiceSvc.BLL.Interfaces;
using MedicalServiceSvc.DAL.Models;
using MessageBroker.Contracts.StaffContracts.SpecialtyContracts;
using MessageBroker.Events.StaffEvents.SpecialtyEvents;
using SharedLibrary.Base.Data;
using SharedLibrary.Base.Data.SqlServerDb;
using SharedLibrary.Base.MessageBuses;
using SharedLibrary.Base.Services;
using SharedLibrary.Commons.Exceptions;
using SharedLibrary.Commons.Filters;
using SharedLibrary.Commons.Results;
using SharedLibrary.Enums;
using SharedLibrary.Services.Interfaces;

namespace MedicalServiceSvc.BLL.Implements
{
    public class MedicalServiceService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IMessageBus messageBus,
        ICurrentUserService currentUserService)
            : BaseService<MedicalService, UpsertMedicalServiceDTO, UpsertMedicalServiceDTO, GetMedicalServiceDTO>(
                unitOfWork,
                mapper,
                includes: [nameof(MedicalService.MedicalServiceRoles)]),
                IMedicalServiceService
    {
        private readonly IMessageBus _messageBus = messageBus;
        private readonly ICurrentUserService _currentUserService = currentUserService;
        private readonly IGenericRepository<MedicalService> _medicalServiceRepository = unitOfWork.Repository<MedicalService>();
        private readonly IGenericRepository<MedicalServiceRole> _medicalServiceRoleRepository = unitOfWork.Repository<MedicalServiceRole>();

        public override async Task<List<GetMedicalServiceDTO>> GetAllAsync()
        {
            var medicalServices = await _medicalServiceRepository.GetAllAsync(x => x.IsActive, _includes);
            var collection = _mapper.Map<List<GetMedicalServiceDTO>>(medicalServices);
            await this.MapSpecialtyNamesAsync(collection);
            return collection;
        }

        public override async Task<GetMedicalServiceDTO> CreateAsync(UpsertMedicalServiceDTO createDTO)
        {
            await _messageBus.RequestAsync<CheckSpecialtyExistByIdEvent, CheckSpecialtyExistByIdContract>
                (new CheckSpecialtyExistByIdEvent { SpecialtyId = createDTO.SpecialtyId });

            createDTO.Code = this.BuildCodeFromName(createDTO.Name ?? string.Empty);

            var medicalService = _mapper.Map<MedicalService>(createDTO);

            if (createDTO.Roles.Count > 0)
            {
                var distinctRoles = createDTO.Roles
                    .Where(r => !string.IsNullOrWhiteSpace(r))
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToList();

                var requestRoles = distinctRoles
                    .Select(role =>
                        Enum.TryParse<RoleEnum>(role, true, out var parsed)
                            ? parsed
                            : throw new InvalidDataException($"Invalid role: {role}"))
                    .ToHashSet();

                medicalService.MedicalServiceRoles = requestRoles
                    .Select(roleEnum => new MedicalServiceRole
                    {
                        Role = roleEnum
                    })
                    .ToList();
            }

            await _medicalServiceRepository.AddAsync(medicalService);
            await _unitOfWork.SaveChangeAsync();

            return await this.GetByIdAsync(medicalService.Id);
        }

        public override async Task<GetMedicalServiceDTO> UpdateAsync(int id, UpsertMedicalServiceDTO upsertDTO)
        {
            await _messageBus.RequestAsync<CheckSpecialtyExistByIdEvent, CheckSpecialtyExistByIdContract>
                   (new CheckSpecialtyExistByIdEvent { SpecialtyId = upsertDTO.SpecialtyId });

            var medicalService = await _medicalServiceRepository.GetByIdAsync(id, _includes)
                ?? throw new DataNotFoundException(typeof(MedicalService), id);

            _medicalServiceRoleRepository.RemoveRange(medicalService.MedicalServiceRoles.ToList());

            _mapper.Map(upsertDTO, medicalService);

            if (upsertDTO.Roles.Count > 0)
            {
                var distinctRoles = upsertDTO.Roles
                    .Where(r => !string.IsNullOrWhiteSpace(r))
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToList();

                var requestRoles = distinctRoles
                    .Select(role => Enum.TryParse<RoleEnum>(role, true, out var parsed)
                        ? parsed
                        : throw new InvalidDataException($"Invalid role: {role}"))
                    .ToHashSet();

                medicalService.MedicalServiceRoles = requestRoles
                    .Select(roleEnum => new MedicalServiceRole
                    {
                        Role = roleEnum
                    })
                    .ToList();
            }

            _medicalServiceRepository.Update(medicalService);
            await _unitOfWork.SaveChangeAsync();

            return await this.GetByIdAsync(id);
        }

        public override async Task<PaginationResult<GetMedicalServiceDTO>> GetAllPaginatedAsync(FilterDTO filterDTO)
        {
            var getAll = await base.GetAllPaginatedAsync(filterDTO);
            await this.MapSpecialtyNamesAsync(getAll.Collection);
            return getAll;
        }

        public async Task<List<GetMedicalServiceDTO>> GetAllForCurrentUserAsync()
        {
            var entities = await _medicalServiceRepository
                .GetAllAsync(x => x.MedicalServiceRoles != null && x.MedicalServiceRoles.Any(r => r.Role == _currentUserService.Role), _includes);

            var collection = _mapper.Map<List<GetMedicalServiceDTO>>(entities);
            await this.MapSpecialtyNamesAsync(collection);
            return collection;
        }

        public async Task<GetMedicalServiceDTO> GetByCodeAsync(string code)
        {
            var medicalService = await _medicalServiceRepository
                .GetByConditionAsync(x => x.Code == code && x.IsActive, _includes);

            if (medicalService == null)
            {
                throw new DataNotFoundException($"Medical service with code '{code}' not found or inactive");
            }

            return _mapper.Map<GetMedicalServiceDTO>(medicalService);
        }

        public async Task<List<GetMedicalServiceDTO>> GetAllByCodesAsync(List<string> codes)
        {
            var medicalServices = await _medicalServiceRepository
                .GetAllAsync(x => x.Code != null && codes.Contains(x.Code) && x.IsActive, _includes);

            return _mapper.Map<List<GetMedicalServiceDTO>>(medicalServices);
        }

        private async Task MapSpecialtyNamesAsync(List<GetMedicalServiceDTO> collection)
        {
            var specialtyIds = collection.Select(x => x.SpecialtyId).Distinct().ToList();
            if (specialtyIds.Count == 0)
            {
                return;
            }

            var contract = await _messageBus
                .RequestAsync<GetListSpecialtyDataByIdsEvent, GetListSpecialtyDataByIdsContract>(
                    new() { SpecialtyIds = specialtyIds });

            var dict = contract.Data.ToDictionary(x => x.Id, x => x.Name);
            foreach (var item in collection)
            {
                if (dict.TryGetValue(item.SpecialtyId, out var name))
                {
                    item.SpecialtyName = name;
                }
            }
        }

        private string BuildCodeFromName(string name)
        {
            var letters = name
                .ToUpperInvariant()
                .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(w => w[0]);

            var prefix = new string(letters.ToArray());
            if (string.IsNullOrWhiteSpace(prefix))
            {
                prefix = "MS";
            }

            var randomNumber = Random.Shared.Next(100, 1000);

            return $"{prefix}{randomNumber}";
        }
    }
}