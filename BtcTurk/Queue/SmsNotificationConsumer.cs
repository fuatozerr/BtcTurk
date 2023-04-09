using BtcTurk.Dto.QueueDto;
using MassTransit;

namespace BtcTurk.Queue
{
    public class SmsNotificationConsumer : IConsumer<SmsDto>
    {
        public Task Consume(ConsumeContext<SmsDto> context)
        {
            var x = context.Message;
            return Task.CompletedTask;
        }
    }
}
