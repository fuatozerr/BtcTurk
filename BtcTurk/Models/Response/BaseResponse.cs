using System.Net;

namespace BtcTurk.Models.Response
{
    public class BaseResponse
    {
        public bool Success { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string Message { get; set; }

    }
}
