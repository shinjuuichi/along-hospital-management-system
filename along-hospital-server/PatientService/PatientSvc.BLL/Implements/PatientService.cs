using AutoMapper;
using MessageBroker.Contracts.AuthAccountContracts.CreateAccountContracts;
using MessageBroker.Contracts.AuthAccountContracts.GetUserDataContracts;
using MessageBroker.Contracts.AuthAccountContracts.UpdateAccountContracts;
using MessageBroker.Events.AuthAccountEvents;
using MessageBroker.Events.AuthAccountEvents.CreateAccountEvents;
using MessageBroker.Events.AuthAccountEvents.GetUserDataEvents;
using MessageBroker.Events.AuthAccountEvents.UpdateAccountEvents;
using MessageBroker.Events.CartEvents;
using MessageBroker.Events.VoucherEvents;
using PatientSvc.BLL.DTOs;
using PatientSvc.BLL.Interfaces;
using PatientSvc.DAL.Models;
using SharedLibrary.Base.Data;
using SharedLibrary.Base.Data.SqlServerDb;
using SharedLibrary.Base.MessageBuses;
using SharedLibrary.Base.Services;
using SharedLibrary.Commons.Exceptions;
using SharedLibrary.Services.Interfaces;

namespace PatientSvc.BLL.Implements
{
    public class PatientService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IUploadFileService uploadFileService,
        IMessageBus messageBus)
            : BaseService<Patient, CreatePatientDTO, UpdatePatientDTO, GetPatientDTO>(
                unitOfWork,
                mapper,
                uploadFileService,
                ["Allergies"]),
                    IPatientService
    {
        private readonly IMessageBus _messageBus = messageBus;
        private readonly IGenericRepository<Allergy> _allergyRepository = unitOfWork.Repository<Allergy>();

        public override async Task<List<GetPatientDTO>> GetAllAsync()
        {
            var patientDTOs = await base.GetAllAsync();
            var patientIds = patientDTOs.Select(p => p.Id).ToList();

            var userContracts = await _messageBus
                .RequestAsync<GetListUserDataByUserIdsEvent, GetListUserDataByUserIdsContract>(new() { UserIds = patientIds });

            for (int i = 0; i < patientDTOs.Count; i++)
            {
                var userContract = userContracts.Data.FirstOrDefault(u => u.UserId == patientDTOs[i].Id);
                if (userContract != null)
                {
                    patientDTOs[i] = _mapper.Map(userContract, patientDTOs[i]);
                }
            }

            return patientDTOs;
        }

        public override async Task<GetPatientDTO> GetByIdAsync(int id)
        {
            var userContract = await _messageBus
                .RequestAsync<GetUserDataByUserIdEvent, GetUserDataByUserIdContract>(new() { UserId = id });

            var patientDTO = await base.GetByIdAsync(id);
            patientDTO = _mapper.Map(userContract, patientDTO);
            return patientDTO;
        }

        public async Task<GetPatientProfileDTO> GetProfileByPatientIdAsync(int patientId)
        {
            var patient = await _repository.GetByIdAsync(patientId, _includes);
            if (patient == null)
            {
                throw new DataNotFoundException(typeof(Patient), patientId);
            }

            return _mapper.Map<GetPatientProfileDTO>(patient);
        }

        public override async Task<GetPatientDTO> CreateAsync(CreatePatientDTO createDTO)
        {
            var newPatient = _mapper.Map<Patient>(createDTO);

            var createdPatient = await _repository.AddAsync(newPatient);
            await _unitOfWork.SaveChangeAsync();

            await _messageBus.RequestAsync<CreateCartEvent, CreateCartContract>(new() { PatientId = createdPatient.Id });
            await _messageBus.PublishAsync(new CreateVoucherForNewPatientIdEvent { UserId = createdPatient.Id });

            return _mapper.Map<GetPatientDTO>(createdPatient);
        }

        public async Task<GetPatientDTO> CreateWithAccountAsync(CreatePatientAndAccountDTO createPatientAndAccountDTO)
        {
            string? patientImage = null;

            if (createPatientAndAccountDTO.Image != null)
            {
                patientImage = await _uploadFileService!.UploadAsync(createPatientAndAccountDTO.Image, typeof(Patient).Name);
            }

            var createUserEvent = _mapper.Map<CreateUserToAuthEvent>(createPatientAndAccountDTO);
            createUserEvent = createUserEvent with { Image = patientImage };

            int createUserId;
            try
            {
                var createUserContract = await _messageBus.RequestAsync<CreateUserToAuthEvent, CreateUserToAuthContract>(createUserEvent);
                createUserId = createUserContract.Id;
            }
            catch
            {
                await _uploadFileService!.DeleteAsync(patientImage);
                throw;
            }

            var createPatientDTO = _mapper.Map<CreatePatientDTO>(createPatientAndAccountDTO);
            createPatientDTO.Id = createUserId;

            try
            {
                await this.CreateAsync(createPatientDTO);
                return await this.GetByIdAsync(createUserId);
            }
            catch
            {
                await _uploadFileService!.DeleteAsync(patientImage);
                await _messageBus.PublishAsync(new DeleteUserByIdWhenCrashingEvent { Id = createUserId });

                throw;
            }
        }

        public override async Task<GetPatientDTO> UpdateAsync(int id, UpdatePatientDTO updateDTO)
        {
            var patient = await _repository.GetByIdAsync(id, _includes)
                ?? throw new DataNotFoundException(typeof(Patient), id);

            _allergyRepository.RemoveRange(patient.Allergies.ToList());

            _mapper.Map(updateDTO, patient);

            var result = _repository.Update(patient);
            await _unitOfWork.SaveChangeAsync();

            return await this.GetByIdAsync(result.Id);
        }

        public async Task<GetPatientDTO> UpdateWithAccountAsync(int id, UpdatePatientAndAccountDTO updatePatientAndAccountDTO)
        {
            var updateUserEvent = _mapper.Map<UpdateUserToAuthEvent>(updatePatientAndAccountDTO);
            updateUserEvent = updateUserEvent with { UserId = id };

            await _messageBus.RequestAsync<UpdateUserToAuthEvent, UpdateUserToAuthContract>(updateUserEvent);

            var updatePatientDTO = _mapper.Map<UpdatePatientDTO>(updatePatientAndAccountDTO);
            return await this.UpdateAsync(id, updatePatientDTO);
        }

        public async Task<bool> CheckExistByIdAsync(int patientId)
        {
            return await _repository.AnyAsync(p => p.Id == patientId);
        }
    }
}
