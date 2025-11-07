using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Validator.Interfaces.Policies;

namespace Validator.Policies
{
    public class CityPolicy : ICityPolicy
    {
        public (bool isValid, IEnumerable<string> errors) IsAllowed(string cityName)
        {
            var errors = new HashSet<string>();
            if (string.IsNullOrWhiteSpace(cityName))
            {
                errors.Add("City name is required");
                return (false, errors);
            }

            return (true, errors);
        }
    }
}
