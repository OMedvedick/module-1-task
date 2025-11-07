using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Validator.Interfaces.Policies
{
    public interface ICityPolicy
    {
        (bool isValid, IEnumerable<string> errors) IsAllowed(string cityName);
    }
}
