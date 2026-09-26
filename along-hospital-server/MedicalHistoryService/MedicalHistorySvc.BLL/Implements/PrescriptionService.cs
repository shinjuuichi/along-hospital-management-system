using AutoMapper;
using MedicalHistorySvc.BLL.DTOs.PrescriptionDTOs.GetDTOs;
using MedicalHistorySvc.BLL.DTOs.PrescriptionDTOs.UpsertDTOs;
using MedicalHistorySvc.BLL.Interfaces;
using MedicalHistorySvc.DAL.Enums;
using MedicalHistorySvc.DAL.Models;
using MessageBroker.Contracts.MedicineContracts;
using MessageBroker.Events.MedicineEvents;
using SharedLibrary.Base.Data;
using SharedLibrary.Base.Data.SqlServerDb;
using SharedLibrary.Base.MessageBuses;
using SharedLibrary.Commons.Exceptions;
using SharedLibrary.Services.Interfaces;

namespace MedicalHistorySvc.BLL.Implements
{
    public class PrescriptionService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IMessageBus messageBus,
        ICurrentUserService currentUserService)
            : IPrescriptionService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly IMessageBus _messageBus = messageBus;
        private readonly ICurrentUserService _currentUserService = currentUserService;

        private readonly IGenericRepository<Prescription> _prescriptionRepository = unitOfWork.Repository<Prescription>();
        private readonly IGenericRepository<MedicalHistory> _medicalHistoryRepository = unitOfWork.Repository<MedicalHistory>();
        private readonly string[] _includesForMedicalHistory = ["Prescription.PrescriptionDetails"];

        public async Task<GetPrescriptionDTO> CreateAsync(int medicalHistoryId, UpsertPrescriptionDTO upsertPrescriptionDTO)
        {
            var medicalHistory = await _medicalHistoryRepository.GetByIdAsync(medicalHistoryId, _includesForMedicalHistory);
            if (medicalHistory == null)
            {
                throw new DataNotFoundException("Cannot create prescription for an unknown medical history");
            }

            if (medicalHistory.MedicalHistoryStatus != MedicalHistoryStatusEnum.Draft)
            {
                throw new InvalidDataException("Cannot create prescription for a medical history that is not in draft status");
            }

            if (medicalHistory.Prescription != null)
            {
                throw new InvalidDataException("Prescription already exists for this medical history");
            }

            if (medicalHistory.DoctorId != _currentUserService.UserId)
            {
                throw new UnauthorizedAccessException("You are not authorized to create a prescription for this medical history");
            }

            await this.RequestMedicineValueForPrescriptionDetailAsync(upsertPrescriptionDTO);

            var prescription = _mapper.Map<Prescription>(upsertPrescriptionDTO);

            var prescriptionDetails = upsertPrescriptionDTO.PrescriptionDetails
                .Select(_mapper.Map<PrescriptionDetail>)
                .ToList();
            prescription.PrescriptionDetails = prescriptionDetails;
            prescription.MedicalHistoryId = medicalHistoryId;

            await _prescriptionRepository.AddAsync(prescription);
            await _unitOfWork.SaveChangeAsync();

            return _mapper.Map<GetPrescriptionDTO>(prescription);
        }

        public async Task<GetPrescriptionDTO> UpdateByMedicalHistoryIdAsync(int medicalHistoryId, UpsertPrescriptionDTO upsertPrescriptionDTO)
        {
            var medicalHistory = await _medicalHistoryRepository.GetByIdAsync(medicalHistoryId, _includesForMedicalHistory);
            if (medicalHistory == null)
            {
                throw new DataNotFoundException("Cannot update prescription for an unknown medical history");
            }

            if (medicalHistory.DoctorId != _currentUserService.UserId)
            {
                throw new UnauthorizedAccessException("You are not authorized to update the prescription for this medical history");
            }

            var existingPrescription = medicalHistory.Prescription;
            if (existingPrescription == null)
            {
                throw new DataNotFoundException("No existing prescription found for this medical history");
            }

            await this.RequestMedicineValueForPrescriptionDetailAsync(upsertPrescriptionDTO);

            _mapper.Map(upsertPrescriptionDTO, existingPrescription);

            var prescriptionDetails = upsertPrescriptionDTO.PrescriptionDetails
                .Select(_mapper.Map<PrescriptionDetail>)
                .ToList();
            existingPrescription.PrescriptionDetails = prescriptionDetails;

            _prescriptionRepository.Update(existingPrescription);
            await _unitOfWork.SaveChangeAsync();

            return _mapper.Map<GetPrescriptionDTO>(existingPrescription);
        }

        private async Task RequestMedicineValueForPrescriptionDetailAsync(UpsertPrescriptionDTO upsertPrescriptionDTO)
        {
            var distinctMedicineIds = upsertPrescriptionDTO.PrescriptionDetails
                .Select(pd => pd.MedicineId)
                .Distinct()
                .ToList();

            if (distinctMedicineIds.Count == 0)
            {
                throw new InvalidDataException("Prescription must contain at least one medicine");
            }

            // Request medicine data by IDs
            var getListMedicineDataByIdsEvent = new GetListMedicineDataByIdsEvent { Ids = distinctMedicineIds };
            var getListMedicineDataByIdsContract = await _messageBus
                .RequestAsync<GetListMedicineDataByIdsEvent, GetListMedicineDataByIdsContract>(
                    getListMedicineDataByIdsEvent);

            var medicineDict = getListMedicineDataByIdsContract.Data.ToDictionary(m => m.Id);

            // Map medicine data to prescription details
            for (int i = 0; i < upsertPrescriptionDTO.PrescriptionDetails.Count; i++)
            {
                var prescriptionDetailDTO = upsertPrescriptionDTO.PrescriptionDetails[i];
                if (medicineDict.TryGetValue(prescriptionDetailDTO.MedicineId, out var medicine))
                {
                    upsertPrescriptionDTO.PrescriptionDetails[i].MedicineSnapshot = _mapper.Map(medicine, prescriptionDetailDTO.MedicineSnapshot);
                }
            }
        }
    }
}
