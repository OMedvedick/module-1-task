using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Validator.Interfaces.Policies
{
    public interface IAccountNamePolicy
    {
        (bool isValid, IEnumerable<string> errors) IsAllowed(string accountName);

    }
}
