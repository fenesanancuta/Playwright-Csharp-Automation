using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;
using PlaywrightTests.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaywrightTests
{
    public class BasePageTest : PageTest
    {
        protected const string BaseUrl = "https://happy-playpen.lovable.app";

        public LoginPage _loginPage = null!;
        protected TestContext.TestAdapter _currentContext;

        [SetUp]
        public async Task Setup()
        {
            //Login page instantiated
            _loginPage = new LoginPage(Page);

            //Nagivate to Login page before each test
            await Page.GotoAsync($"{BaseUrl}/login");
        }

        public override BrowserNewContextOptions ContextOptions()
        {
            _currentContext = TestContext.CurrentContext.Test;

            return new BrowserNewContextOptions()
            {
                BaseURL = BaseUrl,
                //ReduceMotion does not seem to work very well, hence why it is doubled by Page.SuppressAnimationsAsync(). We'll keep it anyway as a safety net. 
                ReducedMotion = ReducedMotion.Reduce
            };
        }

        public async Task TryAsync(Func<Task> test, Func<Task>? tearDown = null)
        {
            TestContext.Progress.WriteLine("--------------------------------------------------------------------------------------");
            TestContext.Progress.WriteLine($"{DateTime.Now} - Starting Test: {_currentContext.MethodName}");
            TestContext.Progress.WriteLine("--------------------------------------------------------------------------------------");

            try
            {
                await test();
            }
            catch (Exception e)
            {
                if (e.GetType() != typeof(NUnit.Framework.AssertionException) &&
                    e.GetType() != typeof(NUnit.Framework.MultipleAssertException))
                {
                    Assert.Fail($"Failed due to exception: \n{e}");
                }
            }
            finally
            {
                if (tearDown is not null)
                {
                    await tearDown();
                }

                TestContext.Progress.WriteLine("--------------------------------------------------------------------------------------");
                TestContext.Progress.WriteLine($"{DateTime.Now} - Test finished: {_currentContext.MethodName}; Test result: {TestContext.CurrentContext.Result.Outcome.ToString()};");
                TestContext.Progress.WriteLine("--------------------------------------------------------------------------------------");
            }
        }
    }
}
