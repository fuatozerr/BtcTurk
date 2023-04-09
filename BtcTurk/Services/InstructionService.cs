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

        public async Task<Response<InstructionDto>> GetInstructionById(int userId, int instructionId)
        {
            try
            {
                var instruction = await _btcTurkDbContext.Instructions.FirstOrDefaultAsync(x => x.Id == instructionId && x.UserId == userId);
                if (instruction == null)
                {
                    var model = Response<InstructionDto>.Fail("Talimat bulunamadı", HttpStatusCode.NotFound);
                    return model;
                }
                var result = _mapper.Map<InstructionDto>(instruction);
                return Response<InstructionDto>.Success(result, HttpStatusCode.OK);
            }
            catch (Exception exx)
            {

                throw;
            }

        }

        //public async Task<Response<bool>> CancelInstructions(CancelInstructionDto request)
        //{
        //    var instruction = await _btcTurkDbContext.Instructions.FirstOrDefaultAsync(x => x.Id == request.InstructionId);
        //    if (instruction is null)
        //    {
        //        var model = Response<bool>.Fail("Gönderilen Idye ait talimat bulunamamıştır", HttpStatusCode.NotFound);
        //        return model;
        //    }
        //    if (instruction.UserId != request.UserId)
        //    {
        //        var model = Response<bool>.Fail("Talimat bilgileriyle User bilgisi eşleşmiyor.", HttpStatusCode.NotFound);
        //        return model;
        //    }
        //    _btcTurkDbContext.Instructions.Attach(instruction);
        //    instruction.IsActive = false;
        //    await _btcTurkDbContext.SaveChangesAsync();
        //    return Response<bool>.Success(HttpStatusCode.NoContent);
        //}
        public async Task<Response<bool>> CancelInstruction(int userId, CancelInstructionDto request)
        {
            var instruction = await _btcTurkDbContext.Instructions.FirstOrDefaultAsync(x => x.Id == request.InstructionId);
            if (instruction is null)
            {
                var model = Response<bool>.Fail("Gönderilen Idye ait talimat bulunamamıştır", HttpStatusCode.NotFound);
                return model;
            }
            if (instruction.UserId != userId)
            {
                var model = Response<bool>.Fail("Talimat bilgileriyle User bilgisi eşleşmiyor.", HttpStatusCode.NotFound);
                return model;
            }
            _btcTurkDbContext.Instructions.Attach(instruction);
            instruction.IsActive = false;
            await _btcTurkDbContext.SaveChangesAsync();
            return Response<bool>.Success(HttpStatusCode.NoContent);
        }
    }
}
