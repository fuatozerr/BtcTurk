namespace BtcTurk.Models
{
    public sealed class Instruction : BaseEntity
    {
        public int UserId { get; set; }
        public int DayOfMonth { get; set; } //1-28 aralığı
        public double Amount { get; set; }
        // public List<Notification> Notifications { get; set; }
        public bool SendSmsNotification { get; set; }
        public bool SendMailNotification { get; set; }
        public bool SendPushNotification { get; set; }
        public bool IsActive { get; set; } = true;

    }
}
