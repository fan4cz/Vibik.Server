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

    public async Task<List<TaskModel>> GetListActiveUserTasks(string username)
    {
        tasksByUser.TryGetValue(username, out var list);
        return await Task.FromResult(list ?? []);
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

    public async Task<bool> ChangeModerationStatus(string username, string taskId, string moderationStatus)
    {
        throw new NotImplementedException();
    }

    public async Task<List<TaskModel>> GetUserSubmissionHistory(string username)
    {
        throw new NotImplementedException();
    }
}