using System;
using System.Linq;
using Validator.Data.Interfaces;

namespace Validator.Data.Records
{
    public record Company : ICompanyData
    {
        private readonly string _accountName = string.Empty;
        private readonly string _city = string.Empty;

        public required string AccountName {
            get => _accountName;
            init => _accountName = value.Trim().ToUpper(); 
        }

        public required string City {
            get => _city;
            init => _city = value.Trim().ToUpper();  
        }

        public required AllowedCountry Country { get; init; } 

        public override string ToString() => $"{AccountName} в городе {City}, {Country.ToString()}";
    }
}