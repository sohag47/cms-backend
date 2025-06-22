using cms_backend.DTOs.Posts;
using FluentValidation;


namespace cms_backend.Validators
{
    public class PostCreateValidator : AbstractValidator<PostCreateDto>
    {
        public PostCreateValidator()
        {
            RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .Length(5, 150);

            RuleFor(x => x.Slug)
                .NotEmpty().WithMessage("Slug is required.")
                .MaximumLength(100);

            RuleFor(x => x.Content)
                .NotEmpty();

            //RuleFor(x => x.Email)
            //    .NotEmpty()
            //    .EmailAddress();

            //RuleFor(x => x.Phone)
            //    .NotEmpty()
            //    .Matches(@"^\d{10,15}$").WithMessage("Phone must be 10-15 digits.");

            //RuleFor(x => x.DateOfBirth)
            //    .NotNull()
            //    .LessThan(DateTime.Today).WithMessage("Date of birth must be in the past.");

            //RuleFor(x => x.IsActive)
            //    .NotNull().WithMessage("IsActive is required.");
        }
    }
}
