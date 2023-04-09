namespace BtcTurk.Models
{
    public class NotificationLog : BaseEntity
    {
        public LogLevel LogLevel { get; set; }
        public int UserId { get; set; }
        public int InstructionId { get; set; }
        public string NotificationMessage { get; set; }
        public string LogJson { get; set; }
        public string ChannelName { get; set; }

    }
}
