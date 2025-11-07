using System;
using System.Linq;
using NUnit.Framework;
using Validator.Policies;

[TestFixture]
public class AccountNamePolicyTests
{


    [Test]
    public void Constructor_WhenMinLengthIsNegative_ShouldThrowArgumentOutOfRange()
    {


        var ex = Assert.Throws<ArgumentOutOfRangeException>(() =>
            new AccountNamePolicy(-1)
        );



        Assert.That(ex.ParamName, Is.EqualTo("minLength"));
        Assert.That(ex.Message, Does.Contain("cannot be less than 0"));
    }

    [Test]
    public void Constructor_WhenMinLengthIsZero_ShouldCreateInstance()
    {


        Assert.DoesNotThrow(() => new AccountNamePolicy(0));
    }






    [TestCase(null, 3, "Account name is required")]
    [TestCase("", 3, "Account name is required")]
    [TestCase("   ", 3, "Account name is required")]
    [TestCase("aa", 3, "Account name should be at least 3 characters long")]
    [TestCase("abcd", 5, "Account name should be at least 5 characters long")]
    public void IsAllowed_WhenInputIsInvalid_ShouldReturnFalseAndCorrectError(
        string accountName,
        int minLength,
        string expectedError)
    {

        var policy = new AccountNamePolicy(minLength);


        var (isValid, errors) = policy.IsAllowed(accountName);


        Assert.That(isValid, Is.False);
        Assert.That(errors.Count(), Is.EqualTo(1));
        Assert.That(errors.First(), Is.EqualTo(expectedError));
    }




    [TestCase("abc", 3)]
    [TestCase("abcd", 3)]
    [TestCase("go", 0)]
    [TestCase("google", 5)]
    public void IsAllowed_WhenInputIsValid_ShouldReturnTrueAndNoErrors(
        string accountName,
        int minLength)
    {

        var policy = new AccountNamePolicy(minLength);


        var (isValid, errors) = policy.IsAllowed(accountName);


        Assert.That(isValid, Is.True);
        Assert.That(errors, Is.Empty);
    }
}
