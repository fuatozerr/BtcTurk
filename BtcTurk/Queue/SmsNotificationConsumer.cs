using BtcTurk.Dto.QueueDto;
using BtcTurk.Services.Interfaces;
using MassTransit;

namespace BtcTurk.Queue
{
    public class SmsNotificationConsumer : IConsumer<SmsDto>
    {
        private readonly INotificationService _notificationService;

        public SmsNotificationConsumer(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public Task Consume(ConsumeContext<SmsDto> context)
        {
            var instructionDto = context.Message;
            _notificationService.SendSmsNotification(instructionDto.InstructionId);
            return Task.CompletedTask;
        }
    }
}
