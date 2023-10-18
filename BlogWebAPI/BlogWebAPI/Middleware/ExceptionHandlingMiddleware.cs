using BlogWebAPI.Exceptions;
using BlogWebAPI.Results;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace BlogWebAPI.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        RequestDelegate next;
        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;   
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next.Invoke(context);
            }
            catch (Exception ex)
            {
                await HandleException(context, ex);
            }
        }

        private Task HandleException(HttpContext context,Exception ex)
        {
            var code = ex switch
            {
                NotFoundException _ => HttpStatusCode.NotFound,
                BadRequestException _ => HttpStatusCode.BadRequest,
                ConflictException _ => HttpStatusCode.Conflict,
                UnauthorizedException _=> HttpStatusCode.Unauthorized,
                _ => HttpStatusCode.InternalServerError,
            };
            var errors = new List<string>() { ex.Message};

            var result = JsonSerializer.Serialize(ApiResult<string>.Failure(code, errors));
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(result);
        }

    }
}
