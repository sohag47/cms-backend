using FluentValidation;
using cms_backend.DTOs.Users;


namespace cms_backend.Validators
{
    public class RegisterValidator : AbstractValidator<RegisterDto>
    {
        public RegisterValidator()
        {
            RuleFor(x => x.Username).NotEmpty().MinimumLength(3);

            RuleFor(x => x.Email).NotEmpty().EmailAddress();

            RuleFor(x => x.Password).NotEmpty().MinimumLength(6);
        }
    }
}
