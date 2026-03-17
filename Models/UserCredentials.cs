using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PlaywrightTests.Models;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class UserCredentials
{

    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    public static UserCredentials ValidUser() => new()
    {
        Email = "test@demo.com",
        Password = "demo1234"
    };

    public static UserCredentials InvalidUser() => new()
    {
        Email = "wrong@com",
        Password = "wrongpass"
    };

    public static UserCredentials EmptyPassword() => new()
    {
        Email = "test@demo.com",
        Password = ""
    };

    public static UserCredentials EmptyEmail() => new()
    {
        Email = "",
        Password = "demo1234"
    };

    public static UserCredentials UpperCaseEmail() => new()
    {
        Email = "TEST@DEMO.COM",
        Password = "demo1234"
    };

    public static UserCredentials UpperCasePassword() => new()
    {
        Email = "test@demo.com",
        Password = "DEMO1234"
    };


    public static string[] InvalidMultipleEmails =
    {
        "test.com",
        "test @demo.com",
        "test@demo,com",
        "test@@demo.com",
        "TEST@DEMO.COM",
    };
}
