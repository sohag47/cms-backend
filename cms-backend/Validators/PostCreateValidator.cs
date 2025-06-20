using cms_backend.DTOs.Posts;
using FluentValidation;


namespace cms_backend.Validators
{
    public class PostCreateValidator : AbstractValidator<PostCreateDto>
    {
        public PostCreateValidator()
        {
            RuleFor(x => x.Title).NotEmpty().Length(5, 150);

            RuleFor(x => x.Slug).NotEmpty().MaximumLength(100);

            RuleFor(x => x.Content).NotEmpty();

            //RuleFor(x => x.Email).EmailAddress();

            //RuleFor(x => x.Phone).Matches(@"^\+?\d{10,15}$").WithMessage("Invalid phone number");

            //RuleFor(x => x.DateOfBirth).LessThan(DateTime.Now).WithMessage("Date must be in the past");

            //RuleFor(x => x.Photo)
            //.NotNull().WithMessage("Photo is required")
            //.Must(f => f.Length <= 2 * 1024 * 1024).WithMessage("Photo must be less than 2MB");
        }
    }
}
