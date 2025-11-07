
using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using FluentValidation;
using FluentValidation.TestHelper;
using Moq;
using NUnit.Framework;
using Validator.Data.Records;
using Validator.Interfaces.Data;
using Validator.Interfaces.Policies;
using Validator.Validators;

[TestFixture]
public class CompanyValidatorTests
{
    private Mock<ICountryPolicy> _mockCountryPolicy;
    private Mock<IAccountNamePolicy> _mockAccountPolicy;
    private Mock<ICityPolicy> _mockCityPolicy;
    private CompanyValidator _validator;

    [SetUp]
    public void SetUp()
    {

        _mockCountryPolicy = new Mock<ICountryPolicy>();
        _mockAccountPolicy = new Mock<IAccountNamePolicy>();
        _mockCityPolicy = new Mock<ICityPolicy>();


        _mockCountryPolicy.Setup(p => p.IsAllowed(It.IsAny<string>()))
                           .Returns((true, Enumerable.Empty<string>()));

        _mockAccountPolicy.Setup(p => p.IsAllowed(It.IsAny<string>()))
                           .Returns((true, Enumerable.Empty<string>()));

        _mockCityPolicy.Setup(p => p.IsAllowed(It.IsAny<string>()))
                           .Returns((true, Enumerable.Empty<string>()));


        _validator = new CompanyValidator(
            _mockCountryPolicy.Object,
            _mockAccountPolicy.Object,
            _mockCityPolicy.Object);
    }




    [Test]
    public void Validate_WhenAllPoliciesPass_ShouldNotHaveAnyValidationErrors()
    {

        var company = CreateStubCompany("ValidName", "ValidCity", "ValidCountry");


        var result = _validator.TestValidate(company);


        result.ShouldNotHaveAnyValidationErrors();
    }
    [Test]
    public void Validate_WhenCountryPolicyFails_ShouldHaveValidationErrorForCountry()
    {

        var testCountry = "FORBIDDEN_LAND";
        var expectedError = "Страна 'FORBIDDEN_LAND' запрещена.";

        _mockCountryPolicy
            .Setup(p => p.IsAllowed(testCountry))
            .Returns((false, new[] { expectedError }));

        var company = CreateStubCompany("ValidName", "ValidCity", testCountry);


        var result = _validator.TestValidate(company);


        result.ShouldHaveValidationErrorFor(c => c.Country)
              .WithErrorMessage(expectedError);

        result.ShouldNotHaveValidationErrorFor(c => c.AccountName);
        result.ShouldNotHaveValidationErrorFor(c => c.City);
    }




    [Test]
    public void Validate_WhenCityPolicyFails_ShouldHaveValidationErrorForCity()
    {

        var testCity = "BAD_CITY";
        var expectedError = "Город не найден.";

        _mockCityPolicy
            .Setup(p => p.IsAllowed(testCity))
            .Returns((false, new[] { expectedError }));

        var company = CreateStubCompany("ValidName", testCity, "ValidCountry");


        var result = _validator.TestValidate(company);


        result.ShouldHaveValidationErrorFor(c => c.City)
              .WithErrorMessage(expectedError);
    }




    [Test]
    public void Validate_WhenAccountNamePolicyFails_ShouldHaveValidationErrorForAccountName()
    {

        var testName = "INVALID";
        var expectedError = "Имя 'INVALID' уже занято.";

        _mockAccountPolicy
            .Setup(p => p.IsAllowed(testName))
            .Returns((false, new[] { expectedError }));

        var company = CreateStubCompany(testName, "ValidCity", "ValidCountry");


        var result = _validator.TestValidate(company);


        result.ShouldHaveValidationErrorFor(c => c.AccountName)
              .WithErrorMessage(expectedError);
    }





    [Test]
    public void Validate_WhenMultiplePoliciesFail_ShouldHaveAllErrors()
    {

        var countryError = "Запрещенная страна";
        var nameError = "Запрещенное имя";

        _mockCountryPolicy
            .Setup(p => p.IsAllowed("BAD_COUNTRY"))
            .Returns((false, new[] { countryError }));

        _mockAccountPolicy
            .Setup(p => p.IsAllowed("BAD_NAME"))
            .Returns((false, new[] { nameError }));

        var company = CreateStubCompany("BAD_NAME", "ValidCity", "BAD_COUNTRY");


        var result = _validator.TestValidate(company);





        Assert.That(result.Errors.Count, Is.EqualTo(2));

        result.ShouldHaveValidationErrorFor(c => c.Country).WithErrorMessage(countryError);
        result.ShouldHaveValidationErrorFor(c => c.AccountName).WithErrorMessage(nameError);
    }






    [Test]
    public void Validate_WhenPolicyFailsWithoutMessage_ShouldThrowInvalidOperationException()
    {

        var testCountry = "BUGGY_POLICY_COUNTRY";


        _mockCountryPolicy
            .Setup(p => p.IsAllowed(testCountry))
            .Returns((false, Enumerable.Empty<string>()));

        var company = CreateStubCompany("ValidName", "ValidCity", testCountry);




        var ex = Assert.Throws<InvalidOperationException>(() =>
            _validator.TestValidate(company)
        );



        Assert.That(ex.Message, Does.Contain("Country"));
        Assert.That(ex.Message, Does.Contain("with no error messages"));
    }


    private ICompanyData CreateStubCompany(string name, string city, string country)
    {
        var mockCompany = new Mock<ICompanyData>();
        mockCompany.Setup(c => c.AccountName).Returns(name);
        mockCompany.Setup(c => c.City).Returns(city);
        mockCompany.Setup(c => c.Country).Returns(country);
        return mockCompany.Object;
    }

    /// <summary>
    /// Доп тест для проверки нормализации record
    /// </summary>
    [Test]
    public void Test_Fields_Normalization()
    {

        var company = new Company
        {
            AccountName = "validName",
            City = "validCity",
            Country = "validCountry"
        };

        company.AccountName.Should().BeUpperCased();
        company.City.Should().BeUpperCased();
        company.Country.Should().BeUpperCased();
    }

        [Test]
    public void Test_ToString_Normization()
    {

        var company = new Company
        {
            AccountName = "validName",
            City = "validCity",
            Country = "validCountry"
        };

        Assert.That(company.ToString(), Is.EqualTo("VALIDNAME в городе VALIDCITY, VALIDCOUNTRY"));
    }
    
}
