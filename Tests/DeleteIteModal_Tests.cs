using Microsoft.Playwright;
using NUnit.Framework;
using PlaywrightTests.Models;
using PlaywrightTests.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaywrightTests.Tests;


[TestFixture]
[Parallelizable(ParallelScope.Self)]
public class DeleteItemModal_Tests : BasePageTest
{
    public Dashboard_Page _dashboardPage = null!;
    public CreateItem_Modal _createItemModal = null!;
    public DeleteItem_Modal _deleteItemModal = null!;

    [SetUp]
    public async Task Setup()
    {
        //Instantiation of POM for Dashboard page
        _dashboardPage = new Dashboard_Page(Page);

        //Instantiation of POM for Login page
        _loginPage = new LoginPage(Page);

        //Instantiation of POM for Create Item Modal
        _createItemModal = new CreateItem_Modal(Page);

        //Instantiation of POM for Delete Item Modal
        _deleteItemModal = new DeleteItem_Modal(Page);

        //ACT for Login > Reaching Dashboard page
        await _loginPage.LoginAsync(UserCredentials.ValidUser());
    }


    [Test]
    public async Task DeleteItem_ClickOnConfirm()
    {
        //LOOP for creating an item beforehand
        while (await _dashboardPage.CountRowsAsync() == 0)
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
    public async Task DeleteItem_ClickOnCancel()
    {
        //LOOP for creating an item beforehand
        while (await _dashboardPage.CountRowsAsync() == 0)
        {
            await _dashboardPage.ClickCreateItemAsync();
            await _createItemModal.SaveAsync("Test");
        }
        
        //Counting the items
        var counterBefore = await _dashboardPage.CountRowsAsync();

        //Click Cancel on Delete Item modal
        await _dashboardPage.ClickDeleteItemAsync();
        await _deleteItemModal.ClickOnCancelAsync();

        //Verifying that no item has been deleted
        bool isModalDisplayed = await _deleteItemModal.VerifyDeleteModalIsVisibleAsync();
        var countAfter = await _dashboardPage.CountRowsAsync();
        Assert.That(countAfter, Is.EqualTo(counterBefore));
        Assert.That(isModalDisplayed, Is.False);
    }
}
