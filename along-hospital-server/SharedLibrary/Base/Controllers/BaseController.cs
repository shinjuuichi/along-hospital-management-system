using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace SharedLibrary.Base.Controllers
{
    [ApiVersion(1)]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public abstract class BaseController : ControllerBase;
}
