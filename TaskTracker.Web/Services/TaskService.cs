using TaskTracker.Web.Models;

namespace TaskTracker.Web.Services;

public class TaskService
{
    private readonly List<TaskItem> _tasks = [];
    private readonly object _sync = new();

    public IReadOnlyList<TaskItem> GetAll()
    {
        lock (_sync)
            return Array.AsReadOnly(_tasks.ToArray());
    }

    public void Add(string title, TaskPriority priority = TaskPriority.Medium)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Task title cannot be empty.", nameof(title));

        lock (_sync)
            _tasks.Add(new TaskItem { Title = title.Trim(), Priority = priority });
    }
}