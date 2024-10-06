using Auth.Common.Helper;
using Auth.Common.Models.Response;

namespace Auth.Api.Middlewares
{
    public class ValidateTokenMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly Dictionary<string, string> tokenRequiredPaths = new()
        {
            { "/users/password", "PUT" }
        };

        public ValidateTokenMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (IsTokenRequiredEndpoint(context.Request.Path.Value!, context.Request.Method))
            {
                try
                {
                    var token = context.Request.Headers["Authorization"].FirstOrDefault();

                    if (string.IsNullOrEmpty(token))
                    {
                        await SetUnauthorizedError(context, "Token gönderilmelidir");
                    }
                    else
                    {
                        TokenHelper.ValidateToken(token.Replace("Bearer ", ""));
                        await _next(context);
                    }
                }
                catch (Exception)
                {
                    await SetUnauthorizedError(context, "Geçersiz token");
                }
            }
            else
            {
                await _next(context);
            }
        }

        private static async Task SetUnauthorizedError(HttpContext context, string responseMessage)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 401;
            await context.Response.WriteAsJsonAsync(new BaseResponseModel().ToErrorResponse(responseMessage));
        }

        private bool IsTokenRequiredEndpoint(string endpoint, string httpType)
        {
            return tokenRequiredPaths.Any(x => x.Key == endpoint && x.Value == httpType);
        }
    }
}
