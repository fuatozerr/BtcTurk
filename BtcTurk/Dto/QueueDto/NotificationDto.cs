namespace BtcTurk.Dto.QueueDto
{
    public class NotificationDto
    {
        public int InstructionId { get; set; }

    }

    public class SmsDto : NotificationDto
    {

    }
    public class MailDto : NotificationDto
    {

    }
    public class PushNotifyDto : NotificationDto
    {

    }

}
