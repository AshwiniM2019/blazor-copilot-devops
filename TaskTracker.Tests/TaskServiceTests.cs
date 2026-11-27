using TaskTracker.Web.Models;
using TaskTracker.Web.Services;

namespace TaskTracker.Tests;

public class TaskServiceTests
{
    private TaskService CreateFreshService()
    {
        return new TaskService();
    }

    [Fact]
    public void Add_CreatesTaskWithDefaultMediumPriority_WhenPriorityNotSpecified()
    {
        // Arrange
        var service = CreateFreshService();
        var title = "Test task";

        // Act
        service.Add(title);

        // Assert
        var tasks = service.GetAll();
        Assert.Single(tasks);
        Assert.Equal("Test task", tasks[0].Title);
        Assert.Equal(TaskPriority.Medium, tasks[0].Priority);
    }

    [Fact]
    public void Add_CreatesTaskWithSpecifiedHighPriority()
    {
        // Arrange
        var service = CreateFreshService();
        var title = "High priority task";

        // Act
        service.Add(title, TaskPriority.High);

        // Assert
        var tasks = service.GetAll();
        var task = tasks.FirstOrDefault(t => t.Title == "High priority task");
        Assert.NotNull(task);
        Assert.Equal(TaskPriority.High, task.Priority);
    }

    [Fact]
    public void Add_CreatesTaskWithLowPriority()
    {
        // Arrange
        var service = CreateFreshService();

        // Act
        service.Add("Low priority task", TaskPriority.Low);

        // Assert
        var tasks = service.GetAll();
        var task = tasks.FirstOrDefault(t => t.Title == "Low priority task");
        Assert.NotNull(task);
        Assert.Equal(TaskPriority.Low, task.Priority);
    }

    [Fact]
    public void Add_ThrowsArgumentException_WhenTitleIsEmpty()
    {
        // Arrange
        var service = CreateFreshService();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => service.Add(string.Empty));
    }

    [Fact]
    public void Add_ThrowsArgumentException_WhenTitleIsWhitespace()
    {
        // Arrange
        var service = CreateFreshService();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => service.Add("   "));
    }

    [Fact]
    public void Add_TrimsTitle()
    {
        // Arrange
        var service = CreateFreshService();

        // Act
        service.Add("  Task with spaces  ");

        // Assert
        var tasks = service.GetAll();
        var task = tasks.FirstOrDefault(t => t.Title.StartsWith("Task with spaces"));
        Assert.NotNull(task);
        Assert.Equal("Task with spaces", task.Title);
    }

    [Fact]
    public void GetAll_ReturnsReadOnlyList()
    {
        // Arrange
        var service = CreateFreshService();

        // Act
        var tasks = service.GetAll();

        // Assert
        Assert.NotNull(tasks);
        Assert.IsAssignableFrom<IReadOnlyList<TaskItem>>(tasks);
    }
}
