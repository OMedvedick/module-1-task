using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using Validator.Interfaces;
using Validator.Interfaces.Data;

namespace Validator.Validators
{
    public class CompanyValidator : AbstractValidator<ICompanyData>
    {
        private readonly IDataPolicy<string> _countryPolicy;
        private readonly IDataPolicy<string> _accountNamePolicy;
        private readonly IDataPolicy<string> _cityPolicy;

        public CompanyValidator(IDataPolicy<string> countryPolicy,
                                IDataPolicy<string> accountNamePolicy,
                                IDataPolicy<string> cityPolicy)
        {
            _countryPolicy = countryPolicy ?? throw new ArgumentNullException(nameof(countryPolicy));
            _accountNamePolicy = accountNamePolicy ?? throw new ArgumentNullException(nameof(accountNamePolicy));
            _cityPolicy = cityPolicy ?? throw new ArgumentNullException(nameof(cityPolicy));

            RuleFor(x => x.AccountName)
                .Custom((accountName, context) =>
                {
                    ApplyPolicy(accountName, context, _accountNamePolicy.IsAllowed);
                });

            RuleFor(x => x.City)
                .Custom((city, context) =>
                {
                    ApplyPolicy(city, context, _cityPolicy.IsAllowed);
                });

            RuleFor(x => x.Country)
                .Custom((country, context) =>
                {
                    ApplyPolicy(country, context, _countryPolicy.IsAllowed);
                });
        }

        private void ApplyPolicy(
            string propertyValue,
            ValidationContext<ICompanyData> context,
            Func<string, (bool isValid, IEnumerable<string> errors)> policyMethod)
        {
            var (isValid, errorMessages) = policyMethod(propertyValue);

            if (isValid)
            {
                return;
            }

            if (errorMessages != null && errorMessages.Any())
            {
                errorMessages.ToList().ForEach(message => context.AddFailure(message));
            }
            else
            {
                context.AddFailure(
                $"Validation policy for '{context.PropertyName}' returned 'invalid' status with no error messages.");
            }
        }
    }
}
