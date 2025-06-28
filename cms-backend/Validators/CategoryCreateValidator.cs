using cms_backend.DTOs.Categories;
using cms_backend.Models;
using FluentValidation;


namespace cms_backend.Validators
{
    public class CategoryCreateValidator : AbstractValidator<CategoryCreateDto>
    {
        public CategoryCreateValidator()
        {
            RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .Length(5, 150).WithMessage("Name must be between 5 and 150 characters.");

            RuleFor(x => x.Slug)
                .NotEmpty().WithMessage("Slug is required.")
                .MaximumLength(100).WithMessage("Slug must not exceed 100 characters.");

            //RuleFor(x => x.Status)
            //    .NotEmpty().WithMessage("Status is required.")
            //    .MaximumLength(100).WithMessage("Status must not exceed 100 characters.");

            RuleFor(x => x.ParentId)
                .GreaterThanOrEqualTo(0).When(x => x.ParentId.HasValue)
                .WithMessage("ParentId cannot be negative.");
        }
    }
}
