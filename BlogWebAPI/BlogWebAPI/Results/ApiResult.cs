using System.Net;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BlogWebAPI.Results
{
    public class ApiResult<T>
    {
        public bool Succeeded { get; set; }
        public IEnumerable<string> Errors { get; set; }
        public HttpStatusCode Code { get; set; }
        public T Data { get; set; }

        public ApiResult(bool succeeded, HttpStatusCode code, IEnumerable<string> errors, T data)
        {
            Succeeded = succeeded;
            Code = code;
            Errors = errors;
            Data = data;
        }

        public ApiResult()
        {
            
        }
        public static ApiResult<T> Success(T data)
            => new ApiResult<T>(true, HttpStatusCode.OK, Enumerable.Empty<string>(), data);
        public static ApiResult<T> Failure(HttpStatusCode code, IEnumerable<string> errors)
             => new ApiResult<T>(false, code, errors, default);
    }
}
