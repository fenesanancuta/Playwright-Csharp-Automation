using Microsoft.Playwright;
using NUnit.Framework;
using PlaywrightTests.Models;
using PlaywrightTests.Pages;
using RandomString4Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaywrightTests.Tests
{
    public class Dashboard_Tests : BasePageTest
    {
        public DashboardPage _dashboardPage = null!;
        public CreateItemModal _createItemModal = null!;
        public DeleteItemModal _deleteItemModal = null!;
        public GridDashboardPage _gridDashboardPage = null!;
        public EditItemModal _editItemModal = null!;


        [SetUp]
        public async Task Setup()
        {

            //Instantiation of POM for Login page
            _loginPage = new LoginPage(Page);

            //Instantiation of POM for Dashboard page
            _dashboardPage = new DashboardPage(Page);

            //Instantiation of POM for Create Item Modal
            _createItemModal = new CreateItemModal(Page);

            //Instantiation of POM for Delete Item Modal
            _deleteItemModal = new DeleteItemModal(Page);

            //Instantiation of POM for GridDashboard Page
            _gridDashboardPage = new GridDashboardPage(Page);

            //Instantiation of POM for Edit Item Modal
            _editItemModal = new EditItemModal(Page);

            //ACT for Login > Reaching Dashboard page
            await _loginPage.LoginAsync(UserCredentials.ValidUser());
        }

        [Test]
        public async Task CreateItem_CorrectMessageDisplayed()
        {
            //ACT for opening Create Item modal
            await _dashboardPage.ClickCreateItemAsync();

            //ACT for creating a new valid item
            await _createItemModal.SaveAsync("test");

            //Verify that the modal has opened by verifying the heading
            bool hasSuccessfullyCreated = await _dashboardPage.IsNotificationDisplayedAsync();
            string valueMessage = await _dashboardPage.GetNotificationTextAsync();
            Assert.That(hasSuccessfullyCreated, Is.True);
            Assert.That(valueMessage, Is.EqualTo("Item has been created"));
        }

        [Test]
        public async Task DeleteItem_CorrectMessageDisplayed()
        {
            //LOOP for creating an item beforehand
            while (await _gridDashboardPage.CountRowsAsync() == 0)
            {
                await _dashboardPage.ClickCreateItemAsync();
                await _createItemModal.SaveAsync("Test");
            }
            //ACT for opening Delete Item modal
            await _dashboardPage.ClickDeleteItemAsync();

            //ACT for saving Delete Item modal
            await _deleteItemModal.ClickOnConfirmAsync();

            //Validate that the message is successfully displayed when deleting an item
            bool hasSuccessfullyCreated = await _dashboardPage.IsNotificationDisplayedAsync();
            string valueMessage = await _dashboardPage.GetNotificationTextAsync();
            Assert.That(hasSuccessfullyCreated, Is.True);
            Assert.That(valueMessage, Is.EqualTo("Item has been removed"));
        }

        [Test]
        public async Task EditItem_CorrectMessageDisplayed()
        {
            //LOOP for creating an item beforehand
            while (await _gridDashboardPage.CountRowsAsync() == 0)
            {
                await _dashboardPage.ClickCreateItemAsync();
                await _createItemModal.SaveAsync("Test");
            }
            //ACT for Edit Item Modal
            await _dashboardPage.ClickEditItemAsync(0);

            //ACT for saving Edit Item modal
            await _editItemModal.SaveAsync("New Name");

            //Validate that the message is successfully displayed when updating an item
            bool hasSuccessfullyUpdated = await _dashboardPage.IsNotificationDisplayedAsync();
            string valueMessage = await _dashboardPage.GetNotificationTextAsync();
            Assert.That(hasSuccessfullyUpdated, Is.True);
            Assert.That(valueMessage, Is.EqualTo("Item has been updated"));
        }

        [Test]
        public async Task LogOut_Successfully()
        {
            //ACT for selecting Sign Out button
            await _dashboardPage.ClickLogOutAsync();

            //Veryfing that Login page is reached
            Assert.That(Page.Url, Does.Contain("/login"));
        }
    }
}
