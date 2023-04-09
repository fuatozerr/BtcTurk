using AutoMapper;
using BtcTurk.Context;
using BtcTurk.Dto;
using BtcTurk.Models;
using BtcTurk.Services.Interfaces;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace BtcTurk.Services
{
    public class InstructionService : IInstructionService
    {
        private readonly BtcTurkDbContext _btcTurkDbContext;
        private readonly IMapper _mapper;
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
            var executionStrategy = _btcTurkDbContext.Database.CreateExecutionStrategy();
            await executionStrategy.ExecuteAsync(async () =>
            {
                using (var transaction = _btcTurkDbContext.Database.BeginTransaction())
                {
                    try
                    {
                        await _btcTurkDbContext.Instructions.AddAsync(instruction);
                        var instructionDbId = await _btcTurkDbContext.SaveChangesAsync();
                        transaction.Commit();
                        SendQueue(instruction);
                        return Dto.Response<bool>.Success(HttpStatusCode.NoContent);
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return Dto.Response<bool>.Fail("Database tarafında problem oluştu.", HttpStatusCode.InternalServerError);
                    }
                }
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

        public async Task<Dto.Response<Dto.NotificationDto>> GetNotificationById(int userId, int instructionId)
        {
            var instruction = await _btcTurkDbContext.Instructions.FirstOrDefaultAsync(x => x.Id == instructionId && x.UserId == userId);
            if (instruction == null)
            {
                var model = Dto.Response<Dto.NotificationDto>.Fail("Talimat bulunamadı", HttpStatusCode.NotFound);
                return model;
            }
            var result = _mapper.Map<Dto.NotificationDto>(instruction);
            return Dto.Response<Dto.NotificationDto>.Success(result, HttpStatusCode.OK);

        }
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
        private async Task SendQueue(Instruction instruction)
        {
            if (instruction.SendSmsNotification)
            {
                await SendSmsQueue(instruction.Id);
            }
            if (instruction.SendMailNotification)
            {
                await SendMailQueue(instruction.Id);
            }
            if (instruction.SendPushNotification)
            {
                await SendPushNotifyQueue(instruction.Id);
            }
        }
        private async Task SendSmsQueue(int instructionId)
        {
            await _publishEndpoint.Publish<Dto.QueueDto.SmsDto>(new
            {
                InstructionId = instructionId
            });
        }
        private async Task SendMailQueue(int instructionId)
        {
            await _publishEndpoint.Publish<Dto.QueueDto.MailDto>(new
            {
                Id = instructionId
            });
        }
        private async Task SendPushNotifyQueue(int instructionId)
        {
            await _publishEndpoint.Publish<Dto.QueueDto.PushNotifyDto>(new
            {
                Id = instructionId
            });
        }
    }
}
