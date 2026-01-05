using AssetManagement.Application.UseCases.Asset;
using FluentValidation;

namespace AssetManagement.Application.UseCases.Asset.Validators
{
    public class UpdateAssetCommandValidator : AbstractValidator<UpdateAssetCommand>
    {
        public UpdateAssetCommandValidator()
        {
            RuleFor(x => x.Asset.Id)
                .GreaterThan(0).WithMessage("Invalid Asset ID.");

            RuleFor(x => x.Asset.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

            RuleFor(x => x.Asset.Price)
                .GreaterThan(0).WithMessage("Price must be greater than 0.");
        }
    }
}
