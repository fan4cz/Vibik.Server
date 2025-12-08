using Shared.Models;
using Shared.Models.Entities;
using Shared.Models.Enums;

namespace Infrastructure.Interfaces;

public interface IUsersTasksTable
{
    public Task<List<TaskModel>> GetListActiveUserTasks(string username);
    public Task<TaskModel> AddUserTask(string username);
    public Task<TaskModelExtendedInfo?> GetTaskExtendedInfo(string username, string taskId);
    public Task<TaskModelExtendedInfo?> GetTaskExtendedInfo(int id);
    public Task<TaskModel?> GetTaskFullInfo(string taskId, string username);
    public Task<bool> ChangeModerationStatus(string username, string taskId, ModerationStatus moderationStatus);
    public Task<bool> ChangeModerationStatus(int id, ModerationStatus moderationStatus);
    public Task<List<TaskModel>> GetUserSubmissionHistory(string username);

    public Task<bool> AddPhoto(string username, string taskId, string photoName);
    public Task<ModerationTask?> GetModerationTask();
    public Task<TaskModel?> ChangeUserTask(string username, string taskId);
    public Task<ModerationStatus> GetModerationStatus(int id);
}