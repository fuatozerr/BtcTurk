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

        [HttpGet("{userId}/{instructionId}/instruction")]   //3 nolu kullanıcının idsi 1 olan talimatını getir
        public async Task<IActionResult> GetInstruction(int userId, int instructionId)
        {
            var result = await _instructionService.GetInstructionById(userId, instructionId);
            return CreateActionResultInstance(result);
        }

        [HttpGet("{userId}/{instructionId}/notifications")]
        public async Task<IActionResult> GetNotifications(int userId, int instructionId)
        {
            var result = await _instructionService.GetNotificationById(userId, instructionId);
            return CreateActionResultInstance(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post(InstructionDto request)
        {
            var result = await _instructionService.Create(request);
            return CreateActionResultInstance(result);
        }

        [HttpPut("{userId}")]
        public async Task<IActionResult> Put(int userId, CancelInstructionDto request)
        {
            var result = await _instructionService.CancelInstruction(userId, request);
            return CreateActionResultInstance(result);
        }
    }
}
