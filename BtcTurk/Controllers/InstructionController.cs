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
        public async Task<IActionResult> Post(InstructionDto request)
        {   //fluent validation ile yönetilebilir.
            //var instruction = _mapper.Map<Instruction>(request);
            //if (request.DayOfMonth >= 1 || request.DayOfMonth <= 28)
            //{
            //    return BadRequest(new BaseResponse { Success = false, Message = "Kullanıcı ayın 1-28 günleri arası için talimat verebilir", StatusCode = HttpStatusCode.BadRequest });
            //}
            //if (request.Amount < MinAmount || request.Amount > MaxAmount)
            //{
            //    return BadRequest(new BaseResponse { Success = false, Message = "minimum 100 TL’lik maksimum 20.000 TL", StatusCode = HttpStatusCode.BadRequest });
            //}
            //var isActive = _btcTurkDbContext.Instructions.Any(x => x.UserId == request.UserId && x.IsActive);
            //if (isActive)
            //{
            //    return Conflict(new BaseResponse { Success = false, Message = "Bir kullanıcıya ait sadece 1 tane aktif talimat olabilir", StatusCode = HttpStatusCode.Conflict });
            //}
            //_btcTurkDbContext.Instructions.Add(instruction);
            //_btcTurkDbContext.SaveChanges();

            return NoContent();
        }
    }
}
