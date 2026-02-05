using FluentValidation;

namespace Business.Models.Validators
{
    public class ProductValidator : AbstractValidator<Product>
    {
        public ProductValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty();

            RuleFor(p => p.Description)
                .NotEmpty();

            RuleFor(p => p.Price)
                .GreaterThan(0);
        }
    }
}
