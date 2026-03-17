using Microsoft.Playwright;
using PlaywrightTests.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlaywrightTests.Pages
{
    public class LoginPage
    {
        private readonly IPage _page;
        private ILocator _usernameInput => _page.GetByLabel("Email");
        private ILocator _passwordInput => _page.GetByLabel("Password");
        private ILocator _SignInButton => _page.GetByRole(AriaRole.Button, new() { Name = "Sign in" });
        private ILocator _errorMessageAlert => _page.GetByRole(AriaRole.Alert);

        public LoginPage(IPage Page)
        {
            _page = Page;
        }
        public async Task LoginAsync(UserCredentials User)
        {
            await _usernameInput.FillAsync(User.Email);
            await _passwordInput.FillAsync(User.Password);
            await _SignInButton.ClickAsync();
        }
        public async Task<bool> IsErrorDisplayedAsync()
        {
            return await _errorMessageAlert.IsVisibleAsync();
        }

        public async Task <string> GetErrorMessageAsync()
        {
           return await _errorMessageAlert.TextContentAsync();
        }
    }
}
