namespace BtcTurk.Services.Interfaces
{
    public interface INotificationService
    {
        void SendSmsNotification(int instructionId);
        //void SendMailNotification(int instructionId);
        //void SendPushNotification(int instructionId);

    }
}
