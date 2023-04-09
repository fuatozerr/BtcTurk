namespace BtcTurk.Dto
{
    public class NotificationDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public bool SendSmsNotification { get; set; }
        public bool SendMailNotification { get; set; }
        public bool SendPushNotification { get; set; }
    }
}
