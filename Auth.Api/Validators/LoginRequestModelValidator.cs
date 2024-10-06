using Auth.Common.Models.Request;
using FluentValidation;

namespace Auth.Api.Validators
{
    public class LoginRequestModelValidator : AbstractValidator<LoginRequestModel>
    {
        public LoginRequestModelValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email bilgisi boş olamaz")
                .EmailAddress().WithMessage("Email formatı hatalı");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Şifre bilgisi boş olamaz");
        }
    }
}
