using System.Net;

namespace BtcTurk.Dto
{
    public class Response<T>
    {
        public T Data { get; private set; }

        public HttpStatusCode StatusCode { get; private set; }
        public bool IsSuccessful { get; private set; }
        public string Message { get; set; }
        public List<string> Errors { get; set; }

        public static Response<T> Success(T data, HttpStatusCode statusCode)
        {
            return new Response<T> { Data = data, StatusCode = statusCode, IsSuccessful = true };
        }

        public static Response<T> Success(HttpStatusCode statusCode)
        {
            return new Response<T> { Data = default(T), StatusCode = statusCode, IsSuccessful = true };
        }

        public static Response<T> Fail(List<string> errors, string message, HttpStatusCode statusCode)

        {
            return new Response<T>
            {
                Errors = errors,
                StatusCode = statusCode,
                IsSuccessful = false,
                Message = message
            };
        }

        public static Response<T> Fail(string error, HttpStatusCode statusCode)
        {
            return new Response<T> { Errors = new List<string>() { error }, StatusCode = statusCode, IsSuccessful = false };
        }
    }
}
