using Auth.Common.Settings;
using System.Net;
using System.Text.RegularExpressions;

namespace Auth.Common.Helper
{
    public class PasswordPolicyHelper
    {
        private readonly PasswordPolicySettings _passwordPolicySettings;

        public PasswordPolicyHelper(PasswordPolicySettings passwordPolicySettings)
        {
            _passwordPolicySettings = passwordPolicySettings;
        }

        public void CheckPasswordPolicy(string password)
        {
            if (password.Length < _passwordPolicySettings.MinLengthCount)
            {
                throw new CustomException($"Şifreniz en az {_passwordPolicySettings.MinLengthCount} karakterden oluşmalıdır", HttpStatusCode.BadRequest);
            }

            if (_passwordPolicySettings.HasSpecialCharacter && !ContainsSpecialCharacters(password))
            {
                throw new CustomException("Şifreniz özel karater içermelidir", HttpStatusCode.BadRequest);
            }

            if (!ContainsUpperAndLower(password))
            {
                throw new CustomException($"Şifreniz en az {_passwordPolicySettings.MinUppercaseCount} büyük, en az {_passwordPolicySettings.MinLowercaseCount} küçük harf içermelidir.", HttpStatusCode.BadRequest);
            }
        }

        private bool ContainsSpecialCharacters(string password)
        {
            string pattern = @"[^a-zA-Z0-9]";

            return Regex.IsMatch(password, pattern);
        }

        private bool ContainsUpperAndLower(string password)
        {
            string pattern = @"^(?=.*[a-z])(?=.*[A-Z]).+$";

            return Regex.IsMatch(password, pattern);
        }
    }
}
