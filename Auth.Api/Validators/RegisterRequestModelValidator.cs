using Auth.Common.Models.Request;
using FluentValidation;

namespace Auth.Api.Validators
{
    public class RegisterRequestModelValidator : AbstractValidator<RegisterRequestModel>
    {
        public RegisterRequestModelValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("İsim bilgisi boş olamaz");

            RuleFor(x => x.Surname)
                .NotEmpty().WithMessage("Soyisim bilgisi boş olamaz");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email bilgisi boş olamaz")
                .EmailAddress().WithMessage("Email formatı hatalı");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Şifre bilgisi boş olamaz")
                .MinimumLength(6).WithMessage("Şifre en az 6 karakterli olmalıdır");

            RuleFor(x => x.Gender)
                .IsInEnum().WithMessage("Geçerli bir cinsiyet seçmelisiniz");
        }
    }
}
