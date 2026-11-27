# TaskTracker Features

## Task Priority Levels

### Overview
Tasks can now be assigned priority levels (`Low`, `Medium`, `High`) to help organize and prioritize work. By default, new tasks are created with `Medium` priority.

### Components & Models

#### `TaskPriority` Enum
**Location:** `TaskTracker.Web/Models/TaskPriority.cs`

Defines the three priority levels:
- `Low` – Blue badge, least urgent
- `Medium` – Yellow badge, default priority
- `High` – Red badge, most urgent

#### `TaskItem` Model
**Location:** `TaskTracker.Web/Models/TaskItem.cs`

Updated with:
```csharp
public TaskPriority Priority { get; set; } = TaskPriority.Medium;
```

Each task stores its priority level and defaults to `Medium`.

### Service Layer

#### `TaskService.Add()` Method
**Location:** `TaskTracker.Web/Services/TaskService.cs`

**Method Signature:**
```csharp
public void Add(string title, TaskPriority priority = TaskPriority.Medium)
```

**Parameters:**
- `title` (string) – The task title (required, non-empty)
- `priority` (TaskPriority) – The task priority (optional, defaults to `Medium`)

**Behavior:**
- Validates that the title is not empty; throws `ArgumentException` if invalid
- Creates a new `TaskItem` with the specified title and priority
- Adds the task to the internal `_tasks` list

### User Interface

#### `Home.razor` Component
**Location:** `TaskTracker.Web/Components/Pages/Home.razor`

**New Features:**

1. **Priority Dropdown**
   - Dropdown (`<select>`) to select priority when adding a new task
   - Default selection is `Medium`
   - Updates the component state via `@bind="_selectedPriority"`

2. **Priority Badges**
   - Each task displays a colored Bootstrap badge showing its priority
   - Badge colors:
     - `Low` → Blue (`bg-info`)
     - `Medium` → Yellow (`bg-warning`)
     - `High` → Red (`bg-danger`)
   - Badges are right-aligned next to task titles using flexbox

3. **Layout Improvements**
   - Input field, priority dropdown, and "Add" button are arranged horizontally using Bootstrap grid (`row g-2`)
   - Task list uses `list-group` for better styling
   - Task items use flexbox to align title and badge

**Helper Method:**
```csharp
private string GetPriorityCssClass(TaskPriority priority) => priority switch
{
    TaskPriority.Low => "bg-info",
    TaskPriority.Medium => "bg-warning",
    TaskPriority.High => "bg-danger",
    _ => "bg-secondary"
};
```

Maps priority levels to Bootstrap badge CSS classes for consistent, semantic styling.

### Testing Considerations

- **Service Tests (xUnit):**
  - Test `Add()` with different priority values
  - Verify default priority is `Medium` when not specified
  - Ensure priority is preserved in the returned task list

- **Component Tests (bUnit):**
  - Test priority dropdown state changes
  - Verify selected priority is passed to `TaskService.Add()`
  - Confirm badge CSS classes match priority levels
  - Test that priority selection resets after adding a task

### Future Enhancements

- Persist priorities to a database
- Filter or sort tasks by priority
- Bulk priority updates
- Priority-based notifications or reminders
- Priority history/audit log
