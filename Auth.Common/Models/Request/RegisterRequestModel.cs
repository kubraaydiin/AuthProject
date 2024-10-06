using Auth.Common.Enums;

namespace Auth.Common.Models.Request
{
    public class RegisterRequestModel
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public GenderType Gender { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
