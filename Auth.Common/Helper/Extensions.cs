using Auth.Common.Models.Response;
using System.Security.Cryptography;
using System.Text;

namespace Auth.Common.Helper
{
    public static class Extensions
    {
        public static string ToHashedData(this string plainText)
        {
            using SHA256 sha256Hash = SHA256.Create();
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(plainText));

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }

            return builder.ToString();
        }

        public static T ToSuccessResponse<T>(this T model, string? message = null) where T : BaseResponseModel
        {
            model.IsSuccess = true;
            model.Message = message ?? "İşlem başarılı";

            return model;
        }

        public static T ToErrorResponse<T>(this T model, string message) where T : BaseResponseModel
        {
            model.IsSuccess = false;
            model.Message = message;

            return model;
        }
    }
}