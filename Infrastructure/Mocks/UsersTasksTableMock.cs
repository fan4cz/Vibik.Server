using Infrastructure.Interfaces;
using Shared.Models;

namespace Infrastructure.Mocks;

public class UsersTasksTableMock : IUsersTasksTable
{
    private readonly Dictionary<string, List<TaskModel>> tasksByUser =
        new(StringComparer.OrdinalIgnoreCase)
        {
            {
                "aboba",
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
                            UserPhotos = ["photoName"],
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
                            UserPhotos = ["photoName"],
                            ExamplePhotos = []
                        }
                    }
                ]
            }
        };

    public async Task<List<TaskModel>> GetListActiveUserTasks(string username)
    {
        tasksByUser.TryGetValue(username, out var list);
        return await Task.FromResult(list ?? []);
    }

    public Task<bool> AddUserTask(string username)
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

    Task<List<TaskModel>> IUsersTasksTable.GetUserSubmissionHistory(string username)
    {
        throw new NotImplementedException();
    }

    public Task<bool> AddPhotoName(string username, string taskId, string photoName)
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