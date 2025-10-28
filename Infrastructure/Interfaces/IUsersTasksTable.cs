using Shared.Models;
using Task = Shared.Models.Task;

namespace Infrastructure.Interfaces;

public interface IUsersTasksTable
{
    public Task<List<Task>> GetListActiveUserTasks(string username);
    public Task<bool> AddUserTask(string username, Task task);
    public Task<TaskExtendedInfo> GetTaskExtendedInfo(string username, string taskId);
    public Task<TaskExtendedInfo> GetTaskExtendedInfo(int id);
    public Task<Task?> GetTaskFullInfo(string taskId, string username);
    public Task<bool> ChangeModerationStatus(string username, string taskId, string moderationStatus);
    public Task<bool> GetUserSubmissionHistory(string username);
}