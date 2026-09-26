using MedicalServiceSvc.BLL.DTOs;
using MedicalServiceSvc.BLL.Interfaces;
using SharedLibrary.Base.Controllers;

namespace MedicalServiceSvc.WebAPI.Controllers
{
    public class MedicalServiceController(IMedicalServiceService medicalServiceService)
        : GetController<GetMedicalServiceDTO, MedicalServiceFilterDTO>(medicalServiceService);
}