using Auth.Common.Helper;
using FluentValidation;
using System.Net;

namespace Auth.Api.Validators
{
    public class GenericValidator
    {
        public void Validate<Validator, Request>(Request request) where Validator : AbstractValidator<Request>
        {
            var validator = (Validator)Activator.CreateInstance(typeof(Validator));

            var validationResult = validator.Validate(request);

            if (!validationResult.IsValid)
            {
                throw new CustomException(validationResult.Errors.FirstOrDefault()?.ErrorMessage, HttpStatusCode.BadRequest);
            }
        }
    }
}
