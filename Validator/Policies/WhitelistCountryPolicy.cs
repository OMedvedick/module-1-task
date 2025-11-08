using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Validator.Interfaces.Policies;

namespace Validator.Policies
{
    public class WhitelistCountryPolicy : ICountryPolicy
    {

        private readonly HashSet<string> _allowedCountries;



        public WhitelistCountryPolicy(IEnumerable<string> allowedCountries)
        {
            _allowedCountries = new HashSet<string>(allowedCountries); //Сюда можно поместитить любой список стран, например "БЕЛАРУСЬ" или "РОССИЯ")

        }


        public (bool isValid, IEnumerable<string> errors) IsAllowed(string countryName)
        {
            var errors = new HashSet<string>();
            var isValid = _allowedCountries.Contains(countryName);
            if (!isValid)
            {
                errors.Add($"Country {countryName} is not allowed");
            }
            return (isValid, errors);

        }

    }
}
