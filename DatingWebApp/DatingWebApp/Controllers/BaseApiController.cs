using DatingWebApp.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace DatingWebApp.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    [ApiController]
    [Route("api/[controller]")]
    public class BaseApiController:ControllerBase
    {

    }
}
