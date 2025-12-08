using Infrastructure.Interfaces;
using Shared.Models;
using Shared.Models.Entities;
using Shared.Models.Enums;

namespace Infrastructure.Mocks;

public class UsersTasksTableMock : IUsersTasksTable
{
    private readonly Dictionary<string, List<TaskModel>> tasksByUser =
        new(StringComparer.OrdinalIgnoreCase)
        {
            {
                "string",
                [
                    new TaskModel
                    {
                        UserTaskId = 1,
                        TaskId = "honey_cars",
                        Name = "Медовые машины",
                        StartTime = DateTime.UtcNow,
                        Reward = 10,
                        ExtendedInfo = new TaskModelExtendedInfo
                        {
                            Description = "Сфоткать 3 желтые машины",
                            PhotosRequired = 3,
                            UserPhotos = [new Uri("https://picsum.photos/seed/moderation/400/300")],
                            ExamplePhotos = []
                        }
                    },
                    new TaskModel
                    {
                        UserTaskId = 2,
                        TaskId = "grass",
                        Name = "Трава",
                        StartTime = DateTime.UtcNow,
                        Reward = 11,
                        ExtendedInfo = new TaskModelExtendedInfo
                        {
                            Description = "Сфоткать траву",
                            PhotosRequired = 1,
                            UserPhotos = [new Uri("https://picsum.photos/seed/moderation/400/300")],
                            ExamplePhotos = []
                        }
                    }
                ]
            }
        };

    private IUsersTasksTable _usersTasksTableImplementation;

    public async Task<List<TaskModel>> GetListActiveUserTasks(string username)
    {
        tasksByUser.TryGetValue(username, out var list);
        return await Task.FromResult(list ?? []);
    }

    public Task<TaskModel> AddUserTask(string username)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> AddUserTask(string username, TaskModel taskModel)
    {
        throw new NotImplementedException();
    }

    public async Task<TaskModelExtendedInfo> GetTaskExtendedInfo(string username, string taskId)
    {
        throw new NotImplementedException();
    }

    public async Task<TaskModelExtendedInfo> GetTaskExtendedInfo(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<TaskModel?> GetTaskFullInfo(string taskId, string username)
    {
        if (!tasksByUser.TryGetValue(username, out var list))
            return null;

        return list.FirstOrDefault(t =>
            string.Equals(t.TaskId, taskId, StringComparison.OrdinalIgnoreCase));
    }

    public Task<bool> ChangeModerationStatus(string username, string taskId, ModerationStatus moderationStatus)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ChangeModerationStatus(int id, ModerationStatus moderationStatus)
    {
        return _usersTasksTableImplementation.ChangeModerationStatus(id, moderationStatus);
    }

    Task<List<TaskModel>> IUsersTasksTable.GetUserSubmissionHistory(string username)
    {
        throw new NotImplementedException();
    }

    public Task<bool> AddPhoto(string username, string taskId, string photoName)
    {
        throw new NotImplementedException();
    }

    public Task<ModerationTask?> GetModerationTask()
    {
        throw new NotImplementedException();
    }

    public async Task<bool> ChangeModerationStatus(string username, string taskId, string moderationStatus)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> GetUserSubmissionHistory(string username)
    {
        throw new NotImplementedException();
    }
}