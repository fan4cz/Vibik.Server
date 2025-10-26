using Infrastructure.Interfaces;
using Shared.Models;
using Task = Shared.Models.Task;

namespace Infrastructure.DataAccess;

public class UserTaskTable : IUserTaskTable
{
    public List<Task> GetListActiveUserTasks(string username)
    {
        throw new NotImplementedException();
    }

    public bool AddUserTask(string username, Task task)
    {
        throw new NotImplementedException();
    }

    public TaskExtendedInfo GetTaskExtendedInfo(string username, string taskId)
    {
        throw new NotImplementedException();
    }
    
    public TaskExtendedInfo GetTaskExtendedInfo(int id)
    {
        throw new NotImplementedException();
    }

    public Task GetTaskFullInfo(int id)
    {
        throw new NotImplementedException();
    }
    
    public Task GetTaskFullInfo(string taskId,  string username)
    {
        throw new NotImplementedException();
    }

    public bool ChangeModerationStatus(string username, string taskId, string  moderationStatus)
    {
        throw new NotImplementedException();
    }

    public bool GetUserSubmissionHistory(string username)
    {
        throw new NotImplementedException();
    }
}