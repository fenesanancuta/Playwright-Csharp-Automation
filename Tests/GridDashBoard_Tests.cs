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
    public class GridDashBoard_Tests : BasePageTest
    {
        public DashboardPage _dashboardPage = null!;
        public CreateItemModal _createItemModal = null!;
        public DeleteItemModal _deleteItemModal = null!;
        public GridDashboardPage _gridDashboardPage = null!;

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

            //ACT for Login > Reaching Dashboard page
            await _loginPage.LoginAsync(UserCredentials.ValidUser());
        }

        [Test]
        public async Task Grid_NoItemsCreated_CorrectMessageDisplayed()
        {
            while (await _gridDashboardPage.CountRowsAsync() > 0)
            {
                await _dashboardPage.ClickDeleteItemAsync();
                await _deleteItemModal.ClickOnConfirmAsync();
            }
            bool hasTheMessageDisplayed = await _gridDashboardPage.IsNoItemsMessageAsync();
            Assert.That(hasTheMessageDisplayed, Is.True);
        }

        [Test]
        public async Task Grid_CreateOneItem_OneRowIsCreated()
        {
            //LOOP for emptying out the grid
            while (await _gridDashboardPage.CountRowsAsync() > 0)
            {
                await _dashboardPage.ClickDeleteItemAsync();
                await _deleteItemModal.ClickOnConfirmAsync();
            }
            //Initiazing a random value for the new item
            string value = RandomString.GetString(Types.ALPHABET_LOWERCASE, 10);

            //Creating a new item using the random value
            await _dashboardPage.ClickCreateItemAsync();
            await _createItemModal.SaveAsync(value);

            //Veiryfing that a new row has been added
            var count = await _gridDashboardPage.CountRowsAsync();
            Assert.That(count, Is.EqualTo(1));
        }

        [Test]
        public async Task Grid_CreateOneMoreItem_RowAreAdded()
        {
            //LOOP for creating at least one item
            while (await _gridDashboardPage.CountRowsAsync() == 0)
            {
                await _dashboardPage.ClickCreateItemAsync();
                await _createItemModal.SaveAsync("Test");
            }

            //Counting the initial rows
            var counterBefore = await _gridDashboardPage.CountRowsAsync();

            //Creating a new item
            await _dashboardPage.ClickCreateItemAsync();
            await _createItemModal.SaveAsync("Test2");

            //Verifying that the initial row count has been updated + 1
            var countAfter = await _gridDashboardPage.CountRowsAsync();
            Assert.That(countAfter, Is.EqualTo(counterBefore + 1));
        }

        [Test]
        public async Task CreateItem_VerifyTheNameRow_SuccessfullyAdded()
        {
            await _dashboardPage.ClickCreateItemAsync();

            //Initiazing a random value for the new item
            string valueRandom = RandomString.GetString(Types.ALPHABET_LOWERCASE, 20);

            //ACT for creating the new item
            await _createItemModal.SaveAsync(valueRandom);

            //Verifying that the correct name has been assigned
            string valueOfTheRow = await _gridDashboardPage.NameLastRowAsync();
            Assert.That(valueOfTheRow, Is.EqualTo(valueRandom));
        }

        [Test]
        public async Task EmptyGrid_TryingToDeleteAnItem()
        {
            //LOOP for emptying out the grid
            while (await _gridDashboardPage.CountRowsAsync() > 0)
            {
                await _dashboardPage.ClickDeleteItemAsync();
                await _deleteItemModal.ClickOnConfirmAsync();
            }

            //ACT for clicking on Delete button
            await _dashboardPage.ClickDeleteItemAsync();
            await _deleteItemModal.ClickOnConfirmAsync();

            //Variables for checking 
            bool hasNoItemsMessageDisplayed = await _gridDashboardPage.IsNoItemsMessageAsync();
            bool hasTheDeleteValidationMessageDisplayed = await _gridDashboardPage.IsItemDeletedAsync();

            //Verify that the correct error message is displayed
            Assert.That(hasNoItemsMessageDisplayed, Is.True);
            Assert.That(hasTheDeleteValidationMessageDisplayed, Is.True);
        }
    }
}
