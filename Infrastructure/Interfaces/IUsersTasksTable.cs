using Shared.Models;

namespace Infrastructure.Interfaces;

public interface IUsersTasksTable
{
    public Task<List<TaskModel>> GetListActiveUserTasks(string username);
    public Task<bool> AddUserTask(string username, TaskModel taskModel);
    public Task<TaskModelExtendedInfo> GetTaskExtendedInfo(string username, string taskId);
    public Task<TaskModelExtendedInfo> GetTaskExtendedInfo(int id);
    public Task<TaskModel?> GetTaskFullInfo(string taskId, string username);
    public Task<bool> ChangeModerationStatus(string username, string taskId, string moderationStatus);
    public Task<List<TaskModel>> GetUserSubmissionHistory(string username);
}