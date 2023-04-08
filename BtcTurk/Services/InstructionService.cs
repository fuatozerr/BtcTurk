using AutoMapper;
using BtcTurk.Context;
using BtcTurk.Dto;
using BtcTurk.Models;
using BtcTurk.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
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
        public async Task<Response<bool>> Create(InstructionDto request)
        {
            var instruction = _mapper.Map<Instruction>(request);
            var isActive = await _btcTurkDbContext.Instructions.AnyAsync(x => x.UserId == request.UserId && x.IsActive);
            if (isActive)
            {
                var model = Response<bool>.Fail("Bir kullanıcıya ait sadece 1 tane aktif talimat olabilir", HttpStatusCode.Conflict);
                return model;
            }
            await _btcTurkDbContext.Instructions.AddAsync(instruction);
            await _btcTurkDbContext.SaveChangesAsync();
            return Response<bool>.Success(HttpStatusCode.NoContent);
        }

        public List<Instruction> GetInstructions()
        {
            var result = _btcTurkDbContext.Instructions.ToList();
            return result;
        }
    }
}
