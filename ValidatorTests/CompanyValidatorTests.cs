using NUnit.Framework;
using FluentValidation.TestHelper;
using Validator.Validators;
using Validator.Data.Records;
using Validator.Data.Interfaces;

namespace ValidatorTests;

[TestFixture]
public class CompanyValidatorTests
{
    private CompanyValidator _validator = null!;

    [SetUp]
    public void SetUp()
    {
        _validator = new CompanyValidator();
    }

    [Test]
    public void Should_Have_Error_When_AccountName_Is_Empty()
    {
        var company = new Company { AccountName = "", City = "Test", Country = AllowedCountry.РОССИЯ };
        var result = _validator.TestValidate(company);
        result.ShouldHaveValidationErrorFor(x => x.AccountName)
            .WithErrorMessage("Account name is required");
    }

    [Test]
    public void Should_Have_Error_When_AccountName_Is_Shorter_Than_5()
    {
        var company = new Company { AccountName = "Test", City = "test", Country = AllowedCountry.РОССИЯ };
        var result = _validator.TestValidate(company);
        result.ShouldHaveValidationErrorFor(x => x.AccountName)
            .WithErrorMessage("Account name should be at least 5 characters long");
    }

    [Test]
    public void Should_Not_Have_Error_When_AccountName_Is_Valid()
    {
        var company = new Company { AccountName = "ValidName", City = "Test", Country = AllowedCountry.РОССИЯ };
        var result = _validator.TestValidate(company);
        result.ShouldNotHaveValidationErrorFor(x => x.AccountName);
    }

    [Test]
    public void Should_Have_Error_When_City_Is_Empty()
    {
        var company = new Company { AccountName = "ValidName", City = "", Country = AllowedCountry.РОССИЯ };
        var result = _validator.TestValidate(company);
        result.ShouldHaveValidationErrorFor(x => x.City)
            .WithErrorMessage("City is required");
    }

    [Test]
    public void Should_Have_Error_When_Country_Is_Invalid()
    {
        var invalidCountryValue = (AllowedCountry)(-1);
        var invalidCompany = new Company
        {
            AccountName = "ValidName",
            City = "ValidCity",
            Country = invalidCountryValue
        };

        var result = _validator.TestValidate(invalidCompany);
        result.ShouldHaveValidationErrorFor(x => x.Country).WithErrorMessage("Invalid country");
    }

    [Test]
    public void Should_Not_Have_Error_When_Country_Is_Valid()
    {
        var validCompany = new Company
        {
            AccountName = "ValidName",
            City = "ValidCity",
            Country = AllowedCountry.РОССИЯ
        };
        var result = _validator.TestValidate(validCompany);
        result.ShouldNotHaveValidationErrorFor(x => x.Country);
    }

    // Вообще, как мне кажется, это тестировать не очень-то и надо,
    // но в задании было условие на UPPERCASE и отсутствие лишних пробелов для всех полей, так что вот.
    // Этот тест проверяет логику нормализации в самой записи Company, а не валидатор.
    [Test]
    public void Company_Properties_Should_Be_Normalized()
    {
        var company = new Company
        {
            AccountName = "  MixedCaseName  ",
            City = " city with spaces  ",
            Country = AllowedCountry.РОССИЯ
        };

        Assert.That(company.AccountName, Is.EqualTo("MIXEDCASENAME"));
        Assert.That(company.City, Is.EqualTo("CITY WITH SPACES"));
    }

    [Test]
    public void Company_ToString_Should_Be_Normalized()
    {
        var company = new Company
        {
            AccountName = "  MixedCaseName  ",
            City = " city with spaces  ",
            Country = AllowedCountry.РОССИЯ
        };

        Assert.That(company.ToString(), Is.EqualTo("MIXEDCASENAME в городе CITY WITH SPACES, РОССИЯ"));
    }
}
