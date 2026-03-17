using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaywrightTests.Pages
{
    public class DeleteItem_Modal
    {
        private readonly IPage _page;
        private ILocator _headingTitle => _page.GetByRole(AriaRole.Heading, new() { Name = "Delete Item" });
        private ILocator _cancelButton => _page.GetByRole(AriaRole.Button, new () { Name = "Cancel"});
        private ILocator _confirmButton => _page.GetByRole(AriaRole.Button, new() { Name = "Confirm" });


        public DeleteItem_Modal(IPage page)
        {
            _page = page;
        }

        public async Task<bool> VerifyDeleteModalIsVisibleAsync()
        {
            return await _headingTitle.IsVisibleAsync();
        }

        public async Task ClickOnCancelAsync()
        {
            await _cancelButton.ClickAsync();
        }
        public async Task ClickOnConfirmAsync()
        {
            await _confirmButton.ClickAsync();
        }
    }
}
