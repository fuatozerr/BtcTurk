using BtcTurk.Models;

namespace BtcTurk.Services.Interfaces
{
    public interface ILoggingService
    {
        Task Log(LogLevel logLevel, Instruction entity, string channelName, bool isSuccess);
    }
}
