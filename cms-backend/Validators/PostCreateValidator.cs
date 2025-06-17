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
        }
    }
}
