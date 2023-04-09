using BtcTurk.Context;
using BtcTurk.Models;
using BtcTurk.Services.Interfaces;
using System.Text.Json;

namespace BtcTurk.Services
{
    public class LoggingService : ILoggingService
    {
        private readonly BtcTurkDbContext _btcTurkDbContext;

        public LoggingService(BtcTurkDbContext btcTurkDbContext)
        {
            _btcTurkDbContext = btcTurkDbContext;
        }

        public async Task Log(LogLevel logLevel, Instruction entity, string channelName, bool isSuccess)
        {
            var logMessage = $"{entity.Id} numaralı talimat için {entity.UserId} kullanıcısına {channelName} kanalından {(isSuccess ? "Başarılı" : "Başarısız")} bilgilendirilme mesajı gönderilmiştir.";
            var logEntry = new NotificationLog { LogLevel = logLevel, NotificationMessage = logMessage, ChannelName = channelName, InstructionId = entity.Id, LogJson = JsonSerializer.Serialize(entity), UserId = entity.UserId };
            await _btcTurkDbContext.Set<NotificationLog>().AddAsync(logEntry);
            await _btcTurkDbContext.SaveChangesAsync();
        }
    }
}
