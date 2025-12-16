using Shared.Models.Entities;
using Shared.Models.Enums;

namespace Infrastructure.Interfaces;

public interface IUsersTasksTable
{
    public Task<List<TaskModel>> GetListActiveUserTasks(string username);
    public Task<TaskModelExtendedInfo?> GetTaskExtendedInfo(int id);
    public Task<TaskModel?> GetTaskFullInfo(int taskId);
    public Task<TaskModel?> GetTaskNoExtendedInfo(int id);
    public Task<bool> ChangeModerationStatus(int id, ModerationStatus moderationStatus);
    public Task<List<TaskModel>> GetUserSubmissionHistory(string username);
    public Task<bool> SetPhotos(int id, string[] photosNames);
    public Task<ModerationTask?> GetModerationTask();
    public Task<string> GetModerationStatus(int id);
    public Task<int> GetReward(int id);
}