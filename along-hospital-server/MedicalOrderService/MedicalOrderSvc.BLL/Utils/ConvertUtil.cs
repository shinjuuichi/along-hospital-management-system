using AutoMapper;
using MedicalOrderSvc.BLL.DTOs.ClinicalMedicalOrderDTOs;
using MedicalOrderSvc.BLL.DTOs.InfusionMedicalOrderDTOs;
using MedicalOrderSvc.BLL.DTOs.InstructionMedicalOrderDTOs;
using MedicalOrderSvc.BLL.DTOs.MedicalOrderDTOs;
using MessageBroker.Contracts.MedicalOrderContracts;

namespace MedicalOrderSvc.BLL.Utils
{
    public static class ConvertUtil
    {
        public static List<GetMedicalOrderContract> ConvertMedicalOrderDTOsToContracts(this IMapper mapper, List<GetMedicalOrderDTO> medicalOrderDTOs)
        {
            List<GetMedicalOrderContract> contracts = [];
            foreach (var dto in medicalOrderDTOs)
            {
                switch (dto)
                {
                    case GetInfusionMedicalOrderDTO infusionDto:
                        var infusionContract = mapper.Map<GetInfusionMedicalOrderContract>(infusionDto);
                        contracts.Add(infusionContract);
                        break;
                    case GetInstructionMedicalOrderDTO instructionDto:
                        var instructionContract = mapper.Map<GetInstructionMedicalOrderContract>(instructionDto);
                        contracts.Add(instructionContract);
                        break;
                    case GetClinicalMedicalOrderDTO clinicalDto:
                        var clinicalContract = mapper.Map<GetClinicalMedicalOrderContract>(clinicalDto);
                        contracts.Add(clinicalContract);
                        break;
                    default:
                        break;
                }
            }

            return contracts;
        }
    }
}
