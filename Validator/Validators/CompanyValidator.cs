using FluentValidation;
using Validator.Data;
using Validator.Data.Interfaces;

namespace Validator.Validators
{
    public class CompanyValidator : AbstractValidator<ICompanyData>
    {
        public CompanyValidator()
        {
            RuleFor(x => x.AccountName)
            .NotEmpty()
            .WithMessage("Account name is required")
            .MinimumLength(5)
            .WithMessage("Account name should be at least 5 characters long");

            RuleFor(x => x.City)
            .NotEmpty()
            .WithMessage("City is required");

            RuleFor(x => x.Country)
            .NotEmpty()
            .WithMessage("Country is required")
            .Must(x => Enum.IsDefined(typeof(AllowedCountry), x))
            .WithMessage("Invalid country");
        }



    }
}