using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Validator.Data.Interfaces
{
    //TODO: Replace ENUM with Information Expert
    public enum AllowedCountry
    {
        БЕЛАРУСЬ,
        РОССИЯ

    }
    public interface ICompanyData
    {
        string AccountName { get; }
        string City { get; }
        AllowedCountry Country { get; }
    }
}