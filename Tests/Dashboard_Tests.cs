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
        public Dashboard_Page _dashboardPage = null!;
        public CreateItem_Modal _createItemModal = null!;
        public DeleteItem_Modal _deleteItemModal = null!;

        [SetUp]
        public async Task Setup()
        {

            //Instantiation of POM for Login page
            _loginPage = new LoginPage(Page);

            //Instantiation of POM for Dashboard page
            _dashboardPage = new Dashboard_Page(Page);

            //Instantiation of POM for Create Item Modal
            _createItemModal = new CreateItem_Modal(Page);

            //Instantiation of POM for Delete Item Modal
            _deleteItemModal = new DeleteItem_Modal(Page);

            //ACT for Login > Reaching Dashboard page
            await _loginPage.LoginAsync(UserCredentials.ValidUser());
        }

        [Test]
        public async Task CreateItem_OpeningTheModal()
        {
            //ACT for opening Create Item modal
            await _dashboardPage.ClickCreateItemAsync();

            //Verify that the modal has opened by verifying the heading
            bool isModalOpened = await _createItemModal.VerifyCreateModalIsVisibleAsync();
            Assert.That(isModalOpened, Is.True);
        }

        [Test]
        public async Task DeleteItem_OpeningTheModal()
        {
            //ACT for opening Delete Item modal
            await _dashboardPage.ClickDeleteItemAsync();

            //Verifying that Delete Item Modal is opened
            bool isDeleteModalDisplayed = await _deleteItemModal.VerifyDeleteModalIsVisibleAsync();
            Assert.That(isDeleteModalDisplayed, Is.True);
        }

        [Test]
        public async Task LogOut_Successfully()
        {
            //ACT for selecting Sign Out button
            await _dashboardPage.ClickLogOutAsync();

            //Veryfing that Login page is reached
            Assert.That(Page.Url, Does.Contain("/login"));
        }

        [Test]
        public async Task Grid_NoItemsCreated_CorrectMessageDisplayed()
        {
            while (await _dashboardPage.CountRowsAsync() > 0)
            {
                await _dashboardPage.ClickDeleteItemAsync();
                await _deleteItemModal.ClickOnConfirmAsync();
            }
            bool hasTheMessageDisplayed = await _dashboardPage.IsNoItemsMessageAsync();
            Assert.That(hasTheMessageDisplayed, Is.True);
        }

        [Test]
        public async Task Grid_CreateOneItem_OneRowIsCreated()
        {
            //LOOP for emptying out the grid
            while (await _dashboardPage.CountRowsAsync() > 0)
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
            var count = await _dashboardPage.CountRowsAsync();
            Assert.That(count, Is.EqualTo(1));
        }

        [Test]
        public async Task Grid_CreateOneMoreItem_RowAreAdded()
        {
            //LOOP for creating at least one item
            while (await _dashboardPage.CountRowsAsync() == 0)
            {
                await _dashboardPage.ClickCreateItemAsync();
                await _createItemModal.SaveAsync("Test");
            }

            //Counting the initial rows
            var counterBefore = await _dashboardPage.CountRowsAsync();

            //Creating a new item
            await _dashboardPage.ClickCreateItemAsync();
            await _createItemModal.SaveAsync("Test2");

            //Verifying that the initial row count has been updated + 1
            var countAfter = await _dashboardPage.CountRowsAsync();
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
            string valueOfTheRow = await _dashboardPage.NameLastRowAsync();
            Assert.That(valueOfTheRow, Is.EqualTo(valueRandom));
        }

        [Test]
        public async Task EmptyGrid_TryingToDeleteAnItem()
        {
            //LOOP for emptying out the grid
            while (await _dashboardPage.CountRowsAsync() > 0)
            {
                await _dashboardPage.ClickDeleteItemAsync();
                await _deleteItemModal.ClickOnConfirmAsync();
            }

            //ACT for clicking on Delete button
            await _dashboardPage.ClickDeleteItemAsync();
            await _deleteItemModal.ClickOnConfirmAsync();

            //Variables for checking 
            bool hasNoItemsMessageDisplayed = await _dashboardPage.IsNoItemsMessageAsync();
            bool hasTheDeleteValidationMessageDisplayed = await _dashboardPage.IsItemDeletedAsync();

            //Verify that the correct error message is displayed
            Assert.That(hasNoItemsMessageDisplayed, Is.True);
            Assert.That(hasTheDeleteValidationMessageDisplayed, Is.True);
        }
    }
}
