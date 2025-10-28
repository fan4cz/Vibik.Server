using Infrastructure.Interfaces;
using Shared.Models;
using Task = Shared.Models.Task;

namespace Infrastructure.DataAccess;

public class UsersTasksTable : IUsersTasksTable
{
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

    public async Task<Task?> GetTaskFullInfo(int id)
    {
        throw new NotImplementedException();
    }
    
    public async Task<Task?> GetTaskFullInfo(string taskId,  string username)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> ChangeModerationStatus(string username, string taskId, string  moderationStatus)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> GetUserSubmissionHistory(string username)
    {
        throw new NotImplementedException();
    }
}