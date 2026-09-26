using AutoMapper;
using MedicalHistorySvc.BLL.DTOs.MedicalHistoryDTOs.GetDTOs.MedicalOrderDTOs;
using MessageBroker.Contracts.MedicalOrderContracts;

namespace MedicalHistorySvc.BLL.Utils
{
    public static class ConvertUtil
    {
        public static List<GetMedicalHistoryMedicalOrderDTO> ConvertMedicalOrderContractsToDTOs(this IMapper mapper, List<GetMedicalOrderContract> medicalOrderContracts)
        {
            List<GetMedicalHistoryMedicalOrderDTO> medicalOrderDTOs = [];

            foreach (var contract in medicalOrderContracts)
            {
                switch (contract)
                {
                    case GetClinicalMedicalOrderContract clinicalContract:
                        medicalOrderDTOs.Add(mapper.Map<GetMedicalHistoryClinicalMedicalOrderDTO>(clinicalContract));
                        break;
                    case GetInfusionMedicalOrderContract infusionContract:
                        medicalOrderDTOs.Add(mapper.Map<GetMedicalHistoryInfusionMedicalOrderDTO>(infusionContract));
                        break;
                    case GetInstructionMedicalOrderContract instructionContract:
                        medicalOrderDTOs.Add(mapper.Map<GetMedicalHistoryInstructionMedicalOrderDTO>(instructionContract));
                        break;
                }
            }

            return medicalOrderDTOs;
        }
    }
}
