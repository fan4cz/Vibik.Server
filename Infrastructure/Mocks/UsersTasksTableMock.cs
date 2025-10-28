using Infrastructure.Interfaces;
using Shared.Models;
using Task = Shared.Models.Task;

namespace Infrastructure.Mocks;

public class UsersTasksTableMock : IUsersTasksTable
{
    private readonly Dictionary<string, List<Task>> tasksByUser =
        new(StringComparer.OrdinalIgnoreCase)
        {
            {
                "aboba",
                new List<Task>
                {
                    new Task
                    {
                        TaskId = "honey_cars",
                        Name = "Медовые машины",
                        StartTime = DateTime.UtcNow,
                        Reward = 10,
                        ExtendedInfo = new TaskExtendedInfo
                        {
                            Description = "Сфоткать 3 желтые машины",
                            PhotosRequired = 3,
                            UserPhotos = new List<PhotoModel>
                            {
                                new PhotoModel { Url = "https://picsum.photos/seed/moderation/400/300" }
                            },
                            ExamplePhotos = new List<PhotoModel>()
                        }
                    }
                }
            }
        };

    public async Task<List<Task>> GetListActiveUserTasks(string username)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> AddUserTask(string username, Task task)
    {
        throw new NotImplementedException();
    }

    public async Task<TaskExtendedInfo> GetTaskExtendedInfo(string username, string taskId)
    {
        throw new NotImplementedException();
    }

    public async Task<TaskExtendedInfo> GetTaskExtendedInfo(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<Task?> GetTaskFullInfo(string taskId, string username)
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

    public async Task<bool> GetUserSubmissionHistory(string username)
    {
        throw new NotImplementedException();
    }
}