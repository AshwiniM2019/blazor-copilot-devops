using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Bunit;
using TaskTracker.Web.Components.Pages;
using TaskTracker.Web.Models;
using TaskTracker.Web.Services;
using Microsoft.Extensions.DependencyInjection;

namespace TaskTracker.Tests;

public class HomeRazorTests : TestContext
{
    [Fact]
    public void Home_RendersWithDefaultMediumPriorityInDropdown()
    {
        // Arrange
        var service = new TaskService();
        Services.AddSingleton(service);

        // Act
        var component = RenderComponent<Home>();

        // Assert
        var select = component.Find("select");
        Assert.NotNull(select);
        var selectedOption = component.Find("option[selected]");
        Assert.NotNull(selectedOption);
        Assert.Contains("Medium", selectedOption.TextContent);
    }

    [Fact]
    public void Home_CanSelectHighPriorityFromDropdown()
    {
        // Arrange
        var service = new TaskService();
        Services.AddSingleton(service);
        var component = RenderComponent<Home>();

        // Act
        var select = component.Find("select");
        select.Change(TaskPriority.High.ToString());

        // Assert
        var selectedOption = component.Find("option[selected]");
        Assert.NotNull(selectedOption);
        Assert.Contains("High", selectedOption.TextContent);
    }

    [Fact]
    public void Home_DisplaysValidationErrorWhenAddingEmptyTitle()
    {
        // Arrange
        var service = new TaskService();
        Services.AddSingleton(service);
        var component = RenderComponent<Home>();

        // Act - Try to add empty task
        var button = component.Find("button");
        button.Click();

        // Assert
        var errorMessage = component.Find(".text-danger");
        Assert.NotNull(errorMessage);
        Assert.Contains("Task title cannot be empty", errorMessage.TextContent);
    }

    [Fact]
    public void Home_AddsTaskWithHighPriorityAndDisplaysBadge()
    {
        // Arrange
        var service = new TaskService();
        Services.AddSingleton(service);
        var component = RenderComponent<Home>();

        // Act
        var input = component.Find("input[placeholder='New task title...']");
        input.Change("High priority task");
        
        var select = component.Find("select");
        select.Change(TaskPriority.High.ToString());
        
        var button = component.Find("button");
        button.Click();

        // Assert
        component.WaitForAssertion(() =>
        {
            var badges = component.FindAll(".badge");
            var highBadge = badges.FirstOrDefault(b => b.TextContent.Contains("High"));
            Assert.NotNull(highBadge);
            Assert.Contains("bg-danger", highBadge.ClassName);
        });
    }

    [Fact]
    public void Home_DisplaysCorrectBadgeColorForLowPriority()
    {
        // Arrange
        var service = new TaskService();
        Services.AddSingleton(service);
        var component = RenderComponent<Home>();

        // Act
        var input = component.Find("input[placeholder='New task title...']");
        input.Change("Low priority task");
        
        var select = component.Find("select");
        select.Change(TaskPriority.Low.ToString());
        
        var button = component.Find("button");
        button.Click();

        // Assert
        component.WaitForAssertion(() =>
        {
            var badges = component.FindAll(".badge");
            var lowBadge = badges.FirstOrDefault(b => b.TextContent.Contains("Low"));
            Assert.NotNull(lowBadge);
            Assert.Contains("bg-info", lowBadge.ClassName);
        });
    }

    [Fact]
    public void Home_DisplaysCorrectBadgeColorForMediumPriority()
    {
        // Arrange
        var service = new TaskService();
        Services.AddSingleton(service);
        var component = RenderComponent<Home>();

        // Act
        var input = component.Find("input[placeholder='New task title...']");
        input.Change("Medium priority task");
        
        var button = component.Find("button");
        button.Click();

        // Assert
        component.WaitForAssertion(() =>
        {
            var badges = component.FindAll(".badge");
            var mediumBadge = badges.FirstOrDefault(b => b.TextContent.Contains("Medium"));
            Assert.NotNull(mediumBadge);
            Assert.Contains("bg-warning", mediumBadge.ClassName);
        });
    }

    [Fact]
    public void Home_ClearsInputAfterSuccessfulTaskAddition()
    {
        // Arrange
        var service = new TaskService();
        Services.AddSingleton(service);
        var component = RenderComponent<Home>();

        // Act
        var input = component.Find("input[placeholder='New task title...']") as IHtmlInputElement;
        input.Value = "Test task";
        input.Change("Test task");
        
        var button = component.Find("button");
        button.Click();

        // Assert
        component.WaitForAssertion(() =>
        {
            var updatedInput = component.Find("input[placeholder='New task title...']") as IHtmlInputElement;
            Assert.Empty(updatedInput.Value);
        });
    }

    [Fact]
    public void Home_ResetsPriorityToMediumAfterTaskAddition()
    {
        // Arrange
        var service = new TaskService();
        Services.AddSingleton(service);
        var component = RenderComponent<Home>();

        // Act
        var select = component.Find("select") as IHtmlSelectElement;
        select.Value = TaskPriority.High.ToString();
        select.Change(TaskPriority.High.ToString());
        
        var input = component.Find("input[placeholder='New task title...']");
        input.Change("High task");
        
        var button = component.Find("button");
        button.Click();

        // Assert
        component.WaitForAssertion(() =>
        {
            var updatedSelect = component.Find("select") as IHtmlSelectElement;
            Assert.Equal(TaskPriority.Medium.ToString(), updatedSelect.Value);
        });
    }
}
