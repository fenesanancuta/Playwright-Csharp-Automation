using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaywrightTests.Pages
{
    public class Dashboard_Page
    {
        private readonly IPage _page;
        private ILocator _createItemButton => _page.GetByTestId("create-item");
        private ILocator _deleteItemButton => _page.GetByTestId("delete-item");
        private ILocator _logOutButton => _page.GetByTestId("logout");
        private ILocator _notificationMessages => _page.GetByTestId("success-message");
        private ILocator _noItemWasDeletedMessage => _page.GetByText("No item has been deleted since no items are added on the grid");
        private ILocator _emptyMessage => _page.GetByText("No items yet");
        private ILocator _numberRow => _page.GetByTestId("grid-row-number");
        private ILocator _nameRow => _page.GetByTestId("grid-row-name"); //getByLocator

        public Dashboard_Page(IPage Page)
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

        public async Task ClickLogOutAsync()
        {
            await _logOutButton.ClickAsync();
        }

        public async Task<bool> IsNotificationDisplayedAsync()
        {
            return await _notificationMessages.IsVisibleAsync();
        }

        public async Task <string> GetNotificationTextAsync()
        {
            return await _notificationMessages.TextContentAsync();
        }

        public async Task <bool> IsItemDeletedAsync()
        {
            return await _noItemWasDeletedMessage.IsVisibleAsync();
        }

        public async Task<bool> IsNoItemsMessageAsync()
        {
            return await _emptyMessage.IsVisibleAsync();
        }

        public async Task<int> CountRowsAsync()
        { 
            return await _numberRow.CountAsync(); 
        }

        public async Task<string> NameLastRowAsync()
        {
            return await _nameRow.Last.TextContentAsync();
        }

    }
}
