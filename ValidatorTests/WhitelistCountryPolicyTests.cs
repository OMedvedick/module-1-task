using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Validator.Policies;

[TestFixture]
public class WhitelistCountryPolicyTests
{

    private readonly List<string> _allowedCountries =
        new List<string> { "БЕЛАРУСЬ", "РОССИЯ", };





    [TestCase("БЕЛАРУСЬ")]
    [TestCase("РОССИЯ")]
    public void IsAllowed_WhenCountryIsExactMatch_ShouldReturnTrueAndNoErrors(string testCountry)
    {

        var policy = new WhitelistCountryPolicy(_allowedCountries);


        var (isValid, errors) = policy.IsAllowed(testCountry);


        Assert.That(isValid, Is.True);
        Assert.That(errors, Is.Empty);
    }

    [TestCase("Mexico")]
    [TestCase("France")]
    [TestCase("")]
    [TestCase("   ")]
    [TestCase(null)]
    [TestCase("usa")]
    [TestCase("gErMaNy")]
    [TestCase("CANADA")]
    [TestCase("Germany  ")]
    [TestCase("БЕЛАРУСЬ ")]
    [TestCase("РОССИЯ  ")]
    public void IsAllowed_WhenCountryIsNotExactMatch_ShouldReturnFalseAndError(string testCountry)
    {

        var policy = new WhitelistCountryPolicy(_allowedCountries);
        var expectedError = $"Country {testCountry} is not allowed";


        var (isValid, errors) = policy.IsAllowed(testCountry);


        Assert.That(isValid, Is.False);
        Assert.That(errors.Count(), Is.EqualTo(1));
        Assert.That(errors.First(), Is.EqualTo(expectedError));
    }

    [Test]
    public void IsAllowed_WhenWhitelistIsEmpty_ShouldReturnFalseForAnyCountry()
    {

        var emptyWhitelist = Enumerable.Empty<string>();
        var policy = new WhitelistCountryPolicy(emptyWhitelist);
        var testCountry = "USA";


        var (isValid, errors) = policy.IsAllowed(testCountry);


        Assert.That(isValid, Is.False);
        Assert.That(errors.Count(), Is.EqualTo(1));
        Assert.That(errors.First(), Is.EqualTo($"Country {testCountry} is not allowed"));
    }
}
