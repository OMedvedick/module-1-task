using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Validator.Interfaces.Policies;

namespace Validator.Policies
{
    public class AccountNamePolicy : IAccountNamePolicy
    {
        private readonly int _minLength;

        public AccountNamePolicy(int minLength)
        {
            if (minLength < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(minLength),
                    "Minimal account name length cannot be less than 0");
            }

            _minLength = minLength;
        }

        public (bool isValid, IEnumerable<string> errors) IsAllowed(string accountName)
        {
            var errors = new HashSet<string>();
            if (string.IsNullOrWhiteSpace(accountName))
            {
                errors.Add("Account name is required");
                return (false, errors);
            }

            if (accountName.Length < _minLength)
            {
                errors.Add($"Account name should be at least {_minLength} characters long");
                return (false, errors);
            }

            return (true, errors);
        }
    }
}
