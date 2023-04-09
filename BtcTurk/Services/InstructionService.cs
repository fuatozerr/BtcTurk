using AutoMapper;
using BtcTurk.Context;
using BtcTurk.Dto;
using BtcTurk.Dto.QueueDto;
using BtcTurk.Models;
using BtcTurk.Services.Interfaces;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace BtcTurk.Services
{
    public class InstructionService : IInstructionService
    {
        protected readonly BtcTurkDbContext _btcTurkDbContext;
        protected readonly IMapper _mapper;
        private readonly IPublishEndpoint _publishEndpoint;

        public InstructionService(BtcTurkDbContext btcTurkDbContext, IMapper mapper, IPublishEndpoint publishEndpoint)
        {
            _btcTurkDbContext = btcTurkDbContext ?? throw new ArgumentNullException(nameof(btcTurkDbContext));
            _mapper = mapper;
            _publishEndpoint = publishEndpoint;
        }
        public async Task<Dto.Response<bool>> Create(InstructionDto request)
        {
            var instruction = _mapper.Map<Instruction>(request);
            var isActive = await _btcTurkDbContext.Instructions.AnyAsync(x => x.UserId == request.UserId && x.IsActive);
            if (isActive)
            {
                var model = Dto.Response<bool>.Fail("Bir kullanıcıya ait sadece 1 tane aktif talimat olabilir", HttpStatusCode.Conflict);
                return model;
            }
            await _btcTurkDbContext.Instructions.AddAsync(instruction);
            var iddd = await _btcTurkDbContext.SaveChangesAsync();
            await _publishEndpoint.Publish<SmsDto>(new
            {
                Id = iddd
            });
            return Dto.Response<bool>.Success(HttpStatusCode.NoContent);
        }

        public async Task<Dto.Response<InstructionDto>> GetInstructionById(int userId, int instructionId)
        {
            var instruction = await _btcTurkDbContext.Instructions.FirstOrDefaultAsync(x => x.Id == instructionId && x.UserId == userId);
            if (instruction == null)
            {
                var model = Dto.Response<InstructionDto>.Fail("Talimat bulunamadı", HttpStatusCode.NotFound);
                return model;
            }
            var result = _mapper.Map<InstructionDto>(instruction);
            return Dto.Response<InstructionDto>.Success(result, HttpStatusCode.OK);

        }

        public async Task<Dto.Response<NotificationDto>> GetNotificationById(int userId, int instructionId)
        {
            var instruction = await _btcTurkDbContext.Instructions.FirstOrDefaultAsync(x => x.Id == instructionId && x.UserId == userId);
            if (instruction == null)
            {
                var model = Dto.Response<NotificationDto>.Fail("Talimat bulunamadı", HttpStatusCode.NotFound);
                return model;
            }
            var result = _mapper.Map<NotificationDto>(instruction);
            return Dto.Response<NotificationDto>.Success(result, HttpStatusCode.OK);

        }

        //public async Task<Dto.Response<bool>> CancelInstructions(CancelInstructionDto request)
        //{
        //    var instruction = await _btcTurkDbContext.Instructions.FirstOrDefaultAsync(x => x.Id == request.InstructionId);
        //    if (instruction is null)
        //    {
        //        var model = Dto.Response<bool>.Fail("Gönderilen Idye ait talimat bulunamamıştır", HttpStatusCode.NotFound);
        //        return model;
        //    }
        //    if (instruction.UserId != request.UserId)
        //    {
        //        var model = Dto.Response<bool>.Fail("Talimat bilgileriyle User bilgisi eşleşmiyor.", HttpStatusCode.NotFound);
        //        return model;
        //    }
        //    _btcTurkDbContext.Instructions.Attach(instruction);
        //    instruction.IsActive = false;
        //    await _btcTurkDbContext.SaveChangesAsync();
        //    return Dto.Response<bool>.Success(HttpStatusCode.NoContent);
        //}
        public async Task<Dto.Response<bool>> CancelInstruction(int userId, CancelInstructionDto request)
        {
            var instruction = await _btcTurkDbContext.Instructions.FirstOrDefaultAsync(x => x.Id == request.InstructionId);
            if (instruction is null)
            {
                var model = Dto.Response<bool>.Fail("Gönderilen Idye ait talimat bulunamamıştır", HttpStatusCode.NotFound);
                return model;
            }
            if (instruction.UserId != userId)
            {
                var model = Dto.Response<bool>.Fail("Talimat bilgileriyle User bilgisi eşleşmiyor.", HttpStatusCode.NotFound);
                return model;
            }
            _btcTurkDbContext.Instructions.Attach(instruction);
            instruction.IsActive = false;
            await _btcTurkDbContext.SaveChangesAsync();
            return Dto.Response<bool>.Success(HttpStatusCode.NoContent);
        }
    }
}
