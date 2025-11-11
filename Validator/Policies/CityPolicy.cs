using System.Collections.Generic;
using Validator.Interfaces;

namespace Validator.Policies
{
    public class CityPolicy : IDataPolicy<string>
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
