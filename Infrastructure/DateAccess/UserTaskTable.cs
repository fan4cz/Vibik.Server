using Shared.Models;
using Task = Shared.Models.Task;

namespace Infrastructure.Interfaces;

public class UserTaskTable
{
    public static List<Task> GetListActiveUserTasks(string username)
    {
        throw new NotImplementedException();
    }

    public static bool AddUserTask(string username, Task task)
    {
        throw new NotImplementedException();
    }

    public static TaskExtendedInfo GetTaskExtendedInfo(string username, string taskId)
    {
        throw new NotImplementedException();
    }
    
    public static TaskExtendedInfo GetTaskExtendedInfo(int id)
    {
        throw new NotImplementedException();
    }

    public static Task GetTaskFullInfo(int id)
    {
        throw new NotImplementedException();
    }
    
    public static Task GetTaskFullInfo(string taskId,  string username)
    {
        throw new NotImplementedException();
    }

    public static bool ChangeModerationStatus(string username, string taskId, string  moderationStatus)
    {
        throw new NotImplementedException();
    }

    public static bool GetUserSubmissionHistory(string username)
    {
        throw new NotImplementedException();
    }
}