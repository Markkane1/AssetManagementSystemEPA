using AssetManagement.Application.UseCases.Asset;
using FluentValidation;

namespace AssetManagement.Application.UseCases.Asset.Validators
{
    public class CreateAssetCommandValidator : AbstractValidator<CreateAssetCommand>
    {
        public CreateAssetCommandValidator()
        {
            RuleFor(x => x.Asset.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

            RuleFor(x => x.Asset.Price)
                .GreaterThan(0).WithMessage("Price must be greater than 0.");

            RuleFor(x => x.Asset.Quantity)
                .GreaterThanOrEqualTo(0).WithMessage("Quantity cannot be negative.");

            RuleFor(x => x.Asset.CategoryId)
                .GreaterThan(0).WithMessage("Category is required.");
        }
    }
}
