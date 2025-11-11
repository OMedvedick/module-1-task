using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Validator.Interfaces
{
    public interface IDataPolicy<T>
    {

        (bool isValid, IEnumerable<string> errors) IsAllowed(T field);

    }
}
