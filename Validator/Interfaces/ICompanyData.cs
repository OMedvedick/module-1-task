using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Validator.Interfaces.Data
{

    public interface ICompanyData
    {
        string AccountName { get; }
        string City { get; }
        string Country { get; }
    }
}
