using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;
using PlaywrightTests.Models;
using PlaywrightTests.Pages;

namespace PlaywrightTests.Tests;

[TestFixture]
[Parallelizable(ParallelScope.Self)]
public class LoginTests : BasePageTest
{
    [Test]
    public async Task Login_WithValidData_RedirectToDashBoard()
    {
        //ACT for login into the application using valid credentials
        await _loginPage.LoginAsync(UserCredentials.ValidUser());

        //Verify that Dashboard page is reached
        Assert.That(Page.Url, Does.Contain("/dashboard"));
    }

    [Test]
    public async Task Login_WithInvalidData_RemainOnLogin()
    {
        //ACT for login into the application using invalid credentials
        await _loginPage.LoginAsync(UserCredentials.InvalidUser());

        //Verify that the user remains on Login page
        Assert.That(Page.Url, Does.Contain("/login"));
    }


    [Test]
    public async Task Login_WithInvalidData_CorrectErrorMessageDisplayed()
    {
        //ACT for login into the application using invalid credentials
        await _loginPage.LoginAsync(UserCredentials.InvalidUser());

        //Verify that the correct error message is received
        bool hasErrorDispalyed = await _loginPage.IsErrorDisplayedAsync();
        Assert.That(hasErrorDispalyed, Is.True);
        string valueOfMessage = await _loginPage.GetErrorMessageAsync();    
        Assert.That(valueOfMessage, Is.EqualTo("Invalid email or password"));
    }


    [Test]
    public async Task Login_WithoutPassword_CorrectErrorMessageDisplayed()
    {
        //ACT for login into the application using an empty password
        await _loginPage.LoginAsync(UserCredentials.EmptyPassword());

        //Verify that the correct error message is received
        bool hasErrorDispalyed = await _loginPage.IsErrorDisplayedAsync();
        Assert.That(hasErrorDispalyed, Is.True);
        string valueOfMessage = await _loginPage.GetErrorMessageAsync();
        Assert.That(valueOfMessage, Is.EqualTo("Email and password are required"));
    }

    [Test]
    public async Task Login_WithoutEmail_CorrectErrorMessageDisplayed()
    {
        //ACT for login into the application using an empty email
        await _loginPage.LoginAsync(UserCredentials.EmptyEmail());

        //Verify that the correct error message is received
        bool hasErrorDispalyed = await _loginPage.IsErrorDisplayedAsync();
        Assert.That(hasErrorDispalyed, Is.True);
        string valueOfMessage = await _loginPage.GetErrorMessageAsync();
        Assert.That(valueOfMessage, Is.EqualTo("Email and password are required"));
    }

    [Test]
    public async Task Login_UpperCaseEmail_CorrectErrorMessageDisplayed()
    {
        //ACT for login into the application using an empty email
        await _loginPage.LoginAsync(UserCredentials.UpperCaseEmail());

        //Verify that the correct error message is received
        bool hasErrorDispalyed = await _loginPage.IsErrorDisplayedAsync();
        Assert.That(hasErrorDispalyed, Is.True);
        string valueOfMessage = await _loginPage.GetErrorMessageAsync();
        Assert.That(valueOfMessage, Is.EqualTo("Invalid email or password"));
    }

    [Test]
    public async Task Login_UpperCasePassword_CorrectErrorMessageDisplayed()
    {
        //ACT for login into the application using an empty email
        await _loginPage.LoginAsync(UserCredentials.UpperCasePassword());

        //Verify that the correct error message is received
        bool hasErrorDispalyed = await _loginPage.IsErrorDisplayedAsync();
        Assert.That(hasErrorDispalyed, Is.True);
        string valueOfMessage = await _loginPage.GetErrorMessageAsync();
        Assert.That(valueOfMessage, Is.EqualTo("Invalid email or password"));
    }
}
