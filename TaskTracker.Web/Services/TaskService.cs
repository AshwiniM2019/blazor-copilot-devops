using TaskTracker.Web.Models;

namespace TaskTracker.Web.Services;

public class TaskService
{
    private readonly List<TaskItem> _tasks = [];

    public IReadOnlyList<TaskItem> GetAll() => _tasks.AsReadOnly();

    public void Add(string title, TaskPriority priority = TaskPriority.Medium)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Task title cannot be empty.", nameof(title));

        _tasks.Add(new TaskItem { Title = title.Trim(), Priority = priority });
    }
}