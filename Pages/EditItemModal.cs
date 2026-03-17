using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaywrightTests.Pages
{
    public class EditItemModal
    {
        private readonly IPage _page;
        private ILocator _headingTitle => _page.GetByRole(AriaRole.Heading, new() { Name = "Edit Item" });
        private ILocator _itemNameField => _page.GetByPlaceholder("Item name");
        private ILocator _cancelButton => _page.GetByRole(AriaRole.Button, new() { Name = "Cancel" });
        private ILocator _saveButton => _page.GetByRole(AriaRole.Button, new() { Name = "Save" });
        private ILocator _errorMessageAlert => _page.GetByRole(AriaRole.Alert);

        public EditItemModal(IPage page)
        {
            _page = page;
        }

        public async Task<bool> IsVisibleEditItemAsync()
        {
            return await _headingTitle.IsVisibleAsync();
        }
        public async Task SaveAsync(string itemName)
        {
            await _itemNameField.FillAsync(itemName);
            await _saveButton.ClickAsync();
        }

        public async Task ClickOnCancel()
        {
            await _cancelButton.ClickAsync();
        }

        public async Task <bool> IsErrorDisplayedAsync()
        {
            return await _errorMessageAlert.IsVisibleAsync();
        }

        public async Task <string> GetErrorTextAsync()
        {
            return await _errorMessageAlert.TextContentAsync();
        }

    }
}
