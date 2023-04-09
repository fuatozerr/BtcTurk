using BtcTurk.Dto;
using BtcTurk.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BtcTurk.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstructionController : BaseController
    {
        const int MinAmount = 100;
        const int MaxAmount = 20000;
        public InstructionController(IInstructionService instructionService) : base(instructionService)
        {
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {

            var instructions = _instructionService.GetInstructions();
            return Ok(instructions);

        }

        [HttpPost]
        public async Task<IActionResult> Create(InstructionDto request)
        {
            var result = await _instructionService.Create(request);
            return CreateActionResultInstance(result);
        }
        //[HttpPut]
        //public async Task<IActionResult> Put(CancelInstructionDto request)
        //{
        //    var result = await _instructionService.CancelInstructions(request);
        //    return CreateActionResultInstance(result);
        //}

        [HttpPut("{userId}")]
        public async Task<IActionResult> Put(int userId, CancelInstructionDto request)
        {
            var result = await _instructionService.CancelInstructions(userId, request);
            return CreateActionResultInstance(result);
        }
    }
}
