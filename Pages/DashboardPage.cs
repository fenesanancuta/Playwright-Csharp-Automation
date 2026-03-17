using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaywrightTests.Pages
{
    public class DashboardPage
    {
        private readonly IPage _page;
        private ILocator _createItemButton => _page.GetByTestId("create-item");
        private ILocator _deleteItemButton => _page.GetByTestId("delete-item");
        private ILocator _editItemButton => _page.GetByTestId("edit-item");
        private ILocator _logOutButton => _page.GetByTestId("logout");
        private ILocator _notificationMessages => _page.GetByTestId("success-message");

        public DashboardPage(IPage Page)
        {
            _page = Page;
        }

        public async Task ClickCreateItemAsync()
        {
            await _createItemButton.ClickAsync();
        }

        public async Task ClickDeleteItemAsync()
        {
            await _deleteItemButton.ClickAsync();
        }

        public async Task ClickEditItemAsync(int index)
        { 
            //Using index because we have the Edit button for each row
            await _editItemButton.Nth(index).ClickAsync();
        }

        public async Task ClickLogOutAsync()
        {
            await _logOutButton.ClickAsync();
        }

        public async Task<bool> IsNotificationDisplayedAsync()
        {
            return await _notificationMessages.IsVisibleAsync();
        }

        public async Task<string> GetNotificationTextAsync()
        {
            return await _notificationMessages.TextContentAsync();
        }
    }
}
