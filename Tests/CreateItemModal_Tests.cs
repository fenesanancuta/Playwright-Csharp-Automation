using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;
using PlaywrightTests.Models;
using PlaywrightTests.Pages;
using RandomString4Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PlaywrightTests.Tests;

[TestFixture]
[Parallelizable(ParallelScope.Self)]
public class CreateItemModal_Tests : BasePageTest
{
    public DashboardPage _dashboardPage = null!;
    public CreateItemModal _createItemModal = null!;
    public DeleteItemModal _deleteItemModal = null!;
    public GridDashboardPage _gridDashboardPage = null!;

    [SetUp]
    public async Task Setup()
    {
        //Instantiation of POM for Dashboard page
        _dashboardPage = new DashboardPage(Page);

        //Instantiation of POM for Login page
        _loginPage = new LoginPage(Page);

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
    public async Task CreateItem_SuccessfullyOpenModal()
    {
        //ACT for opening Create Item modal
        await _dashboardPage.ClickCreateItemAsync();

        //Verify that the correct notification message is displayed
        bool isModalOpened = await _createItemModal.VerifyCreateModalIsVisibleAsync();
        Assert.That(isModalOpened, Is.True);
    }


    [Test]
    public async Task CreateItem_EmptyField_GettingCorrectErrorMessage()
    {
        //ACT for opening Create Item modal
        await _dashboardPage.ClickCreateItemAsync();

        //ACT for trying to create an empty item
        await _createItemModal.SaveAsync("");

        //Verify that the correct notification message is displayed
        bool hasNoInput = await _createItemModal.IsErrorDisplayedAsync();
        string valueOfMessage = await _createItemModal.GetErrorTextAsync();
        Assert.That(hasNoInput, Is.True);
        Assert.That(valueOfMessage, Is.EqualTo("An item name should be completed"));
    }

    [Test]
    public async Task CreateItem_Over50Characters_GettingCorrectErrorMessage()
    {
        //ACT for opening Create Item modal
        await _dashboardPage.ClickCreateItemAsync();

        //Creating a variable + 50 characters, creating the item
        string nameOfItem = RandomString.GetString(Types.ALPHABET_LOWERCASE, 51);
        await _createItemModal.SaveAsync(nameOfItem);

        //Verify that the correct notification message is displayed
        bool hasNoInput = await _createItemModal.IsErrorDisplayedAsync();
        string valueOfMessage = await _createItemModal.GetErrorTextAsync();
        Assert.That(hasNoInput, Is.True);
        Assert.That(valueOfMessage, Is.EqualTo("Item name must be 50 characters or less"));
    }

    public async Task CreateItem_50Characters_ItemCreatedSuccessfully()
    {
        //ACT for opening Create Item modal
        await _dashboardPage.ClickCreateItemAsync();

        //Creating a variable of 50 characters, creating the item
        string nameOfItem = RandomString.GetString(Types.ALPHABET_LOWERCASE, 50);
        await _createItemModal.SaveAsync(nameOfItem);

        //Verify that the item is successfully created
        bool hasSuccessfullyCreated = await _dashboardPage.IsNotificationDisplayedAsync();
        string valueMessage = await _dashboardPage.GetNotificationTextAsync();
        Assert.That(hasSuccessfullyCreated, Is.True);
        Assert.That(valueMessage, Is.EqualTo("Item has been created"));
    }

    public async Task CreateItem_Under50Characters_ItemCreatedSuccessfully()
    {
        //ACT for opening Create Item modal
        await _dashboardPage.ClickCreateItemAsync();

        //Creating a variable of 50 characters, creating the item
        string nameOfItem = RandomString.GetString(Types.ALPHABET_LOWERCASE, 49);
        await _createItemModal.SaveAsync(nameOfItem);

        //Verify that the item is successfully created
        bool hasSuccessfullyCreated = await _dashboardPage.IsNotificationDisplayedAsync();
        string valueMessage = await _dashboardPage.GetNotificationTextAsync();
        Assert.That(hasSuccessfullyCreated, Is.True);
        Assert.That(valueMessage, Is.EqualTo("Item has been created"));
    }

    [Test]
    public async Task CreateItem_AddDuplicateItem_CorrectErrorMessageDisplayed()
    {
        //Adding a first item
        await _dashboardPage.ClickCreateItemAsync();
        string nameForFirstItem = RandomString.GetString(Types.ALPHABET_LOWERCASE, 15);
        await _createItemModal.SaveAsync(nameForFirstItem);

        //Variabile for second item, taking the same name as the one used for nameForFirstItem
        string nameForSecondItem = await _gridDashboardPage.NameLastRowAsync();

        //Adding the second item using the same name
        await _dashboardPage.ClickCreateItemAsync();
        await _createItemModal.SaveAsync(nameForSecondItem);

        //Verify that the error message is displayed + verify the message
        bool hasErrorOccured = await _createItemModal.IsErrorDisplayedAsync();
        string valueOfMessage = await _createItemModal.GetErrorTextAsync();

        Assert.That(hasErrorOccured, Is.True);
        Assert.That(valueOfMessage, Is.EqualTo("User cannot add duplicate items"));
    }
}
