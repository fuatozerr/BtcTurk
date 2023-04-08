using AutoMapper;
using BtcTurk.Context;
using BtcTurk.Dto;
using BtcTurk.Models;
using BtcTurk.Models.Response;
using BtcTurk.Services.Interfaces;
using System.Net;

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
        public BaseResponse Create(InstructionDto request)
        {
            var instruction = _mapper.Map<Instruction>(request);
            var isActive = _btcTurkDbContext.Instructions.Any(x => x.UserId == request.UserId && x.IsActive);
            if (isActive)
            {
                return new BaseResponse { Success = false, Message = "Bir kullanıcıya ait sadece 1 tane aktif talimat olabilir", StatusCode = HttpStatusCode.Conflict };
            }

            _btcTurkDbContext.Instructions.Add(instruction);
            _btcTurkDbContext.SaveChanges();

            return new BaseResponse { Success = true, StatusCode = HttpStatusCode.NoContent };
        }

        public List<Instruction> GetInstructions()
        {
            var result = _btcTurkDbContext.Instructions.ToList();
            return result;
        }
    }
}
