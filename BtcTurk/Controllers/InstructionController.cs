using BtcTurk.Dto;
using BtcTurk.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BtcTurk.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstructionController : BaseController
    {
        public InstructionController(IInstructionService instructionService) : base(instructionService)
        {
        }

        /// <summary>
        /// Kullanıcıya ait talimatları getirir
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="instructionId"></param>
        /// <returns></returns>
        [HttpGet("{userId}/{instructionId}/instruction")]   //3 nolu kullanıcının idsi 1 olan talimatını getir
        public async Task<IActionResult> GetInstruction(int userId, int instructionId)
        {
            var result = await _instructionService.GetInstructionById(userId, instructionId);
            return CreateActionResultInstance(result);
        }
        /// <summary>
        /// Kullanıcının talimatlarına ait bildirim kanalları listelenir
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="instructionId"></param>
        /// <returns></returns>
        [HttpGet("{userId}/{instructionId}/notifications")]
        public async Task<IActionResult> GetNotifications(int userId, int instructionId)
        {
            var result = await _instructionService.GetNotificationById(userId, instructionId);
            return CreateActionResultInstance(result);
        }
        /// <summary>
        /// Talimat eklenir
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post(InstructionDto request)
        {
            var result = await _instructionService.Create(request);
            return CreateActionResultInstance(result);
        }
        /// <summary>
        /// Talimatın durumu güncellenir
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("{userId}")]
        public async Task<IActionResult> Put(int userId, CancelInstructionDto request)
        {
            var result = await _instructionService.CancelInstruction(userId, request);
            return CreateActionResultInstance(result);
        }
    }
}
