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
    public DashboardPage _dashboardPage = null!;
    public GridDashboardPage _gridDashboardPage = null!;
    public CreateItemModal _createItemModal = null!;
    public DeleteItemModal _deleteItemModal = null!;

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
    public async Task DeleteItem_SuccessfullyOpenModal()
    {
        //ACT for opening Delete Item modal
        await _dashboardPage.ClickDeleteItemAsync();

        //Verifying that Delete Item Modal is opened
        bool isDeleteModalDisplayed = await _deleteItemModal.VerifyDeleteModalIsVisibleAsync();
        Assert.That(isDeleteModalDisplayed, Is.True);
    }


    [Test]
    public async Task DeleteItem_ClickOnConfirm()
    {
        //LOOP for creating an item beforehand
        while (await _gridDashboardPage.CountRowsAsync() == 0)
        {
            await _dashboardPage.ClickCreateItemAsync();
            await _createItemModal.SaveAsync("Test");
        }

        //Counting the items
        var counterBefore = await _gridDashboardPage.CountRowsAsync();

        //ACT for opening Delete Item modal
        await _dashboardPage.ClickDeleteItemAsync();

        //ACT for saving Delete Item modal
        await _deleteItemModal.ClickOnConfirmAsync();

        //Verifying that no item has been deleted
        bool isModalDisplayed = await _deleteItemModal.VerifyDeleteModalIsVisibleAsync();
        var countAfter = await _gridDashboardPage.CountRowsAsync();
        Assert.That(countAfter, Is.EqualTo(counterBefore - 1));
        Assert.That(isModalDisplayed, Is.True);
    }

    [Test]
    public async Task DeleteItem_ClickOnCancel()
    {
        //LOOP for creating an item beforehand
        while (await _gridDashboardPage.CountRowsAsync() == 0)
        {
            await _dashboardPage.ClickCreateItemAsync();
            await _createItemModal.SaveAsync("Test");
        }

        //Counting the items
        var counterBefore = await _gridDashboardPage.CountRowsAsync();

        //Click Cancel on Delete Item modal
        await _dashboardPage.ClickDeleteItemAsync();
        await _deleteItemModal.ClickOnCancelAsync();

        //Verifying that no item has been deleted
        bool isModalDisplayed = await _deleteItemModal.VerifyDeleteModalIsVisibleAsync();
        var countAfter = await _gridDashboardPage.CountRowsAsync();
        Assert.That(countAfter, Is.EqualTo(counterBefore));
        Assert.That(isModalDisplayed, Is.True);
    }
}
