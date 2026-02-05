using FluentValidation;

namespace Business.Models.Validators
{
    public class AddressValidator : AbstractValidator<Address>
    {
        public AddressValidator()
        {
            RuleFor(a => a.Street1)
                .NotEmpty();

            RuleFor(a => a.City)
                .NotEmpty();

            RuleFor(a => a.State)
                .NotEmpty();

            RuleFor(a => a.PostalCode)
                .NotEmpty();

            RuleFor(a => a.Country)
                .NotEmpty();

        }
    }
}
