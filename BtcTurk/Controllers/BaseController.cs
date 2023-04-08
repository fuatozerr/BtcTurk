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
    }
}
