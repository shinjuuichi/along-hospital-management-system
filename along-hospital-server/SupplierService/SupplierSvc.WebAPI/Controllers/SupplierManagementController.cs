using Microsoft.AspNetCore.Authorization;
using SharedLibrary.Base.Controllers;
using SharedLibrary.Enums;
using SupplierSvc.BLL.DTOs.SupplierDTOs;
using SupplierSvc.BLL.Interfaces;

namespace SupplierSvc.WebAPI.Controllers
{
    [Authorize(Roles = nameof(RoleEnum.InventoryClerk))]
    public class SupplierManagementController(ISupplierService _supplierService)
        : CrudController<UpsertSupplierDTO, UpsertSupplierDTO, GetSupplierDTO>(_supplierService)
    {
        protected override string? EntityName => "Supplier";
    }
}
