using BtcTurk.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BtcTurk.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected readonly IInstructionService _instructionService;

        public BaseController(IInstructionService instructionService)
        {
            _instructionService = instructionService;
        }
        public IActionResult CreateActionResultInstance<T>(BtcTurk.Dto.Response<T> response)
        {
            var obj = new ObjectResult(response)
            {
                StatusCode = (int)response.StatusCode
            };
            return obj;
        }
    }
}
