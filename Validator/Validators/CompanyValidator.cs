using FluentValidation;
using Validator.Data;
using Validator.Interfaces.Data;
using Validator.Interfaces.Policies;

namespace Validator.Validators
{
    public class CompanyValidator : AbstractValidator<ICompanyData>
    {

        private readonly ICountryPolicy _countryPolicy;
        private readonly IAccountNamePolicy _accountNamePolicy;
        private readonly ICityPolicy _cityPolicy;

        public CompanyValidator(ICountryPolicy countryPolicy,
                                IAccountNamePolicy accountNamePolicy,
                                ICityPolicy cityPolicy)
        {

            _countryPolicy = countryPolicy ?? throw new ArgumentNullException(nameof(countryPolicy));
            _accountNamePolicy = accountNamePolicy ?? throw new ArgumentNullException(nameof(accountNamePolicy));
            _cityPolicy = cityPolicy ?? throw new ArgumentNullException(nameof(cityPolicy));


            RuleFor(x => x.AccountName)
                .Custom((accountName, context) =>
                {

                    ApplyPolicy(accountName, context, _accountNamePolicy.IsAllowed, "AccountName");
                });

            RuleFor(x => x.City)
                .Custom((city, context) =>
                {
                    ApplyPolicy(city, context, _cityPolicy.IsAllowed, "City");
                });

            RuleFor(x => x.Country)
                .Custom((country, context) =>
                {
                    ApplyPolicy(country, context, _countryPolicy.IsAllowed, "Country");
                });
        }


        private void ApplyPolicy(
            string propertyValue,
            ValidationContext<ICompanyData> context,
            Func<string, (bool isValid, IEnumerable<string> errors)> policyMethod,
            string propertyName)
        {
            var (isValid, errorMessages) = policyMethod(propertyValue);

            if (isValid)
            {
                return;
            }


            if (errorMessages != null && errorMessages.Any())
            {
                foreach (var message in errorMessages)
                {
                    context.AddFailure(message);
                }
            }
            else
            {
                throw new InvalidOperationException(
                 $"Validation policy for '{propertyName}' returned 'invalid' status " +
                 $"with no error messages.");

            }
        }
    }
}
