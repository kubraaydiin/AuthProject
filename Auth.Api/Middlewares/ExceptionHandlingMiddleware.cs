using Auth.Common.Helper;
using Auth.Common.Models.Response;

namespace Auth.Api.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (CustomException ex)
            {
                await HandleExceptionAsync(context, ex);
            }
            catch
            {
                await HandleUnexpectedError(context);   
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, CustomException exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)exception.StatusCode;
            await context.Response.WriteAsJsonAsync(new BaseResponseModel().ToErrorResponse(exception.Message));
        }

        private async Task HandleUnexpectedError(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 500;
            await context.Response.WriteAsJsonAsync(new BaseResponseModel().ToErrorResponse($"Beklenmeyen bir hata oluştu."));
        }
    }
}
