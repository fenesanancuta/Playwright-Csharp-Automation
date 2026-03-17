using NUnit.Framework;
using PlaywrightTests.Models;
using PlaywrightTests.Pages;
using RandomString4Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaywrightTests.Tests;

[TestFixture]
[Parallelizable(ParallelScope.Self)]
public class EditItemModal_Tests : BasePageTest
{
    public DashboardPage _dashboardPage = null!;
    public CreateItemModal _createItemModal = null!;
    public DeleteItemModal _deleteItemModal = null!;
    public GridDashboardPage _gridDashboardPage = null!;
    public EditItemModal _editItemModal = null!;

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

        //Instantiation of POM for Edit Item Modal
        _editItemModal = new EditItemModal(Page);

        //ACT for Login > Reaching Dashboard page
        await _loginPage.LoginAsync(UserCredentials.ValidUser());
    }

    [Test]
    public async Task EditItem_SuccessfullyOpenModal()
    {
        //LOOP for creating an item
        while (await _gridDashboardPage.CountRowsAsync() == 0)
        {
            await _dashboardPage.ClickCreateItemAsync();
            await _createItemModal.SaveAsync("Test1");
        }

        //ACT for opening Create Item modal
        await _dashboardPage.ClickEditItemAsync(0);

        //Verify that the correct notification message is displayed
        bool isModalOpened = await _editItemModal.IsVisibleEditItemAsync();
        Assert.That(isModalOpened, Is.True);
    }

    [Test]
    public async Task EditItem_SuccessfullyEditTheName()
    {
        //LOOP for creating an item
        while (await _gridDashboardPage.CountRowsAsync() == 0)
        {
            await _dashboardPage.ClickCreateItemAsync();
            await _createItemModal.SaveAsync("Test1");
        }

        string initialName = await _gridDashboardPage.NameLastRowAsync();
        await _dashboardPage.ClickEditItemAsync(0);
        await _editItemModal.SaveAsync("another name");

        string newName = await _gridDashboardPage.NameLastRowAsync();
        Assert.That(newName, Is.EqualTo("another name"));
    }

    [Test]
    public async Task EditItem_EmptyField_GettingCorrectErrorMessage()
    {

        //LOOP for creating at least an item
        while (await _gridDashboardPage.CountRowsAsync() == 0)
        {
            await _dashboardPage.ClickCreateItemAsync();
            await _createItemModal.SaveAsync("Test1");
        }

        //ACT for saving the modal with empty input
        await _dashboardPage.ClickEditItemAsync(0);
        await _editItemModal.SaveAsync("");


        //Verify that the correct notification message is displayed
        bool hasNoInput = await _editItemModal.IsErrorDisplayedAsync();
        string valueOfMessage = await _editItemModal.GetErrorTextAsync();
        Assert.That(hasNoInput, Is.True);
        Assert.That(valueOfMessage, Is.EqualTo("An item name should be completed"));

    }
    [Test]
    public async Task EditItem_EditDuplicateItem_CorrectErrorMessageDisplayed()
    {
        //LOOP for creating at least an item
        while (await _gridDashboardPage.CountRowsAsync() > 0)
        {
            await _dashboardPage.ClickDeleteItemAsync();
            await _deleteItemModal.ClickOnConfirmAsync();
        }

        //Creating 2 variables for 2 new items
        string nameForFirstItem = (RandomString.GetString(Types.ALPHABET_LOWERCASE, 15));
        string nameForSecondItem = (RandomString.GetString(Types.ALPHABET_LOWERCASE, 15));

        //Creating the 2 items, assigning the names
        await _dashboardPage.ClickCreateItemAsync();
        await _createItemModal.SaveAsync(nameForFirstItem);
        await _dashboardPage.ClickCreateItemAsync();
        await _createItemModal.SaveAsync(nameForSecondItem);


        //Adding the second item using the same name
        await _dashboardPage.ClickEditItemAsync(1);
        await _editItemModal.SaveAsync(nameForFirstItem);

        //Verify that the error message is displayed + verify the message
        bool hasErrorOccured = await _editItemModal.IsErrorDisplayedAsync();
        string valueOfMessage = await _editItemModal.GetErrorTextAsync();

        Assert.That(hasErrorOccured, Is.True);
        Assert.That(valueOfMessage, Is.EqualTo("User cannot add duplicate items"));
    }

    [Test]
    public async Task EditItem_Over50Characters_GettingCorrectErrorMessage()
    {
        //LOOP for creating at least an item
        while (await _gridDashboardPage.CountRowsAsync() == 0)
        {
            await _dashboardPage.ClickCreateItemAsync();
            await _createItemModal.SaveAsync("Test1");
        }

        //Variable for 50+ char input
        string nameOfItem = RandomString.GetString(Types.ALPHABET_LOWERCASE, 51);

        //ACT for editing the item's name with 50+ char
        await _dashboardPage.ClickEditItemAsync(0);
        await _editItemModal.SaveAsync(nameOfItem);


        //Verify that the correct notification message is displayed
        bool hasNoInput = await _editItemModal.IsErrorDisplayedAsync();
        string valueOfMessage = await _editItemModal.GetErrorTextAsync();
        Assert.That(hasNoInput, Is.True);
        Assert.That(valueOfMessage, Is.EqualTo("Item name must be 50 characters or less"));
    }

}
