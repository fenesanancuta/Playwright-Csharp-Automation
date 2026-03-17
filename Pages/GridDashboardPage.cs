using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaywrightTests.Pages
{
    public class GridDashboardPage
    {
        private readonly IPage _page;
        private ILocator _noItemWasDeletedMessage => _page.GetByText("No item has been deleted since no items are added on the grid");
        private ILocator _emptyMessage => _page.GetByText("No items yet");
        private ILocator _numberRow => _page.GetByTestId("grid-row-number");
        private ILocator _nameRow => _page.GetByTestId("grid-row-name");

        public GridDashboardPage(IPage Page)
        {
            _page = Page;
        }

        public async Task<bool> IsItemDeletedAsync()
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
