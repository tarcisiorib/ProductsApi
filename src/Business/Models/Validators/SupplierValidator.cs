using FluentValidation;

namespace Business.Models.Validators
{
    public class SupplierValidator : AbstractValidator<Supplier>
    {
        public SupplierValidator()
        {
            RuleFor(s => s.Name)
                .NotEmpty()
                .Length(2, 100);

            RuleFor(s => s.Document)
                .NotEmpty();
        }
    }
}
