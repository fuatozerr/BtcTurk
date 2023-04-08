using AutoMapper;
using BtcTurk.Context;
using BtcTurk.Models;
using BtcTurk.Services.Interfaces;

namespace BtcTurk.Services
{
    public class InstructionService : IInstructionService
    {
        protected readonly BtcTurkDbContext _btcTurkDbContext;
        protected readonly IMapper _mapper;

        public InstructionService(BtcTurkDbContext btcTurkDbContext, IMapper mapper)
        {
            _btcTurkDbContext = btcTurkDbContext ?? throw new ArgumentNullException(nameof(btcTurkDbContext));
            _mapper = mapper;
        }
        public void Create()
        {
            throw new NotImplementedException();
        }

        public List<Instruction> GetInstructions()
        {
            var result = _btcTurkDbContext.Instructions.ToList();
            return result;
        }
    }
}
