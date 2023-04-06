using AutoMapper;
using BtcTurk.Context;
using BtcTurk.Dto;
using BtcTurk.Models;
using Microsoft.AspNetCore.Mvc;

namespace BtcTurk.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstructionController : ControllerBase
    {
        private readonly BtcTurkDbContext _btcTurkDbContext;
        private readonly IMapper _mapper;

        public InstructionController(BtcTurkDbContext btcTurkDbContext, IMapper mapper)
        {
            _btcTurkDbContext = btcTurkDbContext;
            _mapper = mapper;

        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var students = _btcTurkDbContext.Instructions.ToList();
            return Ok(students);
        }

        [HttpPost]
        public async Task<IActionResult> Post(InstructionDto request)
        {
            var instruction = _mapper.Map<Instruction>(request);
            _btcTurkDbContext.Instructions.Add(instruction);
            _btcTurkDbContext.SaveChanges();
            return NoContent();
        }
    }
}
