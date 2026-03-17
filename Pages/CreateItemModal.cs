using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaywrightTests.Pages
{
    public class CreateItemModal
    {
        private readonly IPage _page;
        private ILocator _headingTitle => _page.GetByRole(AriaRole.Heading, new() { Name = "Create Item" });
        private ILocator _itemNameField => _page.GetByPlaceholder("Item name");
        private ILocator _saveButton => _page.GetByRole(AriaRole.Button, new() { Name = "Save" });
        private ILocator _errorMessageAlert => _page.GetByRole(AriaRole.Alert);

        public CreateItemModal(IPage Page)
        {
            _page = Page;
        }
        public async Task <bool> VerifyCreateModalIsVisibleAsync()
        {
            return await _headingTitle.IsVisibleAsync();
        }

        public async Task SaveAsync(string itemName)
        {
            await _itemNameField.FillAsync(itemName);
            await _saveButton.ClickAsync();
        }

        public async Task<bool> IsErrorDisplayedAsync()
        {
            return await _errorMessageAlert.IsVisibleAsync();
        }

        public async Task<string> GetErrorTextAsync()
        {
            return await _errorMessageAlert.TextContentAsync();
        }
    }
}
