using Auth.Api.Attributes;
using Auth.Common.Helper;
using Auth.Common.Models.Response;

namespace Auth.Api.Middlewares
{
    public class ValidateTokenMiddleware
    {
        private readonly RequestDelegate _next;

        public ValidateTokenMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!HasJwtIgnoreAttribute(context))
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

        private async Task SetUnauthorizedError(HttpContext context, string responseMessage)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 401;
            await context.Response.WriteAsJsonAsync(new BaseResponseModel().ToErrorResponse(responseMessage));
        }

        private bool HasJwtIgnoreAttribute(HttpContext context)
        {
            var endpoint = context.GetEndpoint();
            return endpoint?.Metadata.GetMetadata<JwtIgnoreAttribute>() != null;
        }
    }
}
