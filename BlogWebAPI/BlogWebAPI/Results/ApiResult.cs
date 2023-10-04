using System.Net;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BlogWebAPI.Results
{
    public class ApiResult<T>
    {
        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public HttpStatusCode Code { get; set; }
        public T Data { get; set; }

        public ApiResult(bool succeeded, HttpStatusCode code, string message, T data)
        {
            Succeeded = succeeded;
            Code = code;
            Message = message;
            Data = data;
        }

        public ApiResult()
        {
            
        }
        public static ApiResult<T> Success(T data)
            => new ApiResult<T>(true, HttpStatusCode.OK, string.Empty, data);
        public static ApiResult<T> Failure(HttpStatusCode code, string message)
             => new ApiResult<T>(false, code, message, default);
    }
}
