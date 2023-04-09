using BtcTurk.Context;
using BtcTurk.Services.Interfaces;
using System.Net;
using System.Text;

namespace BtcTurk.Services
{
    public class NotificationService : INotificationService
    {
        private readonly BtcTurkDbContext _btcTurkDbContext;
        private const string apiKeySms = @"Njk1NDU3NmI0Nzc3Njk0NDUzNzg2ZDZmNzQzNzQ5NjI=";
        private const string apiUrl = "https://api.txtlocal.com/send/?apikey=";
        private const string numbers = "905522479623";
        private const string message = "Mesaj iletildi";
        private const string sender = "Faruk Fuat Özer";
        private const string contentType = "application/x-www-form-urlencoded";
        private const string channelSms = "SMS";
        private readonly ILoggingService _loggingService;

        public NotificationService(BtcTurkDbContext btcTurkDbContext, ILoggingService loggingService)
        {
            _loggingService = loggingService;
            _btcTurkDbContext = btcTurkDbContext;
        }

        public void SendSmsNotification(int instructionId) //kredi olmadıgı için sms gelmiyor. mail adresime uyarı mailleri düşüyor :)
        {
            var instruction = _btcTurkDbContext.Instructions.FirstOrDefault(x => x.Id == instructionId);

            string result;
            String url = apiUrl + apiKeySms + "&numbers=" + numbers + "&message=" + message + "&sender=" + sender;
            StreamWriter myWriter = null;
            HttpWebRequest objRequest = (HttpWebRequest)WebRequest.Create(url);
            objRequest.Method = "POST";
            objRequest.ContentLength = Encoding.UTF8.GetByteCount(url);
            objRequest.ContentType = contentType;
            try
            {
                myWriter = new StreamWriter(objRequest.GetRequestStream());
                myWriter.Write(url);
                //throw new Exception("test");
            }
            catch (Exception e)
            {
                _loggingService.Log(logLevel: LogLevel.Information, instruction, channelSms, false);
            }
            finally
            {
                myWriter.Close();
            }

            HttpWebResponse objResponse = (HttpWebResponse)objRequest.GetResponse();
            using (StreamReader sr = new StreamReader(objResponse.GetResponseStream()))
            {
                result = sr.ReadToEnd();
                //sms gönderildi log gelecek

                sr.Close();
            }
            _loggingService.Log(logLevel: LogLevel.Information, instruction, channelSms, true);

        }
        public void SendMailNotification(int instructionId)
        {
            throw new NotImplementedException();
        }

        public void SendPushNotification(int instructionId)
        {
            throw new NotImplementedException();
        }


    }
}
