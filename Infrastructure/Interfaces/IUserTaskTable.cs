using Shared.Models;
using Task = System.Threading.Tasks.Task;

namespace Infrastructure.Interfaces;

public interface IUserTaskTable
{
    public List<Task> GetListActiveUserTasks(string username);
    public bool AddUserTask(string username, Task task);
    public TaskExtendedInfo GetTaskExtendedInfo(string username, string taskId);
    public TaskExtendedInfo GetTaskExtendedInfo(int id);
    public Task? GetTaskFullInfo(string taskId, string username);
    public bool ChangeModerationStatus(string username, string taskId, string moderationStatus);
    public bool GetUserSubmissionHistory(string username);
}