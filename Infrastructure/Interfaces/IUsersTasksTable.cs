using Shared.Models;
using Task = Shared.Models.Task;

namespace Infrastructure.Interfaces;

public interface IUsersTasksTable
{
    public List<Task> GetListActiveUserTasks(string username);
    public bool AddUserTask(string username, Task task);
    public TaskExtendedInfo GetTaskExtendedInfo(string username, string taskId);
    public TaskExtendedInfo GetTaskExtendedInfo(int id);
    public Task? GetTaskFullInfo(string taskId, string username);
    public bool ChangeModerationStatus(string username, string taskId, string moderationStatus);
    public bool GetUserSubmissionHistory(string username);
}