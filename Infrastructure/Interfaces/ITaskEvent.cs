using Shared.Models.Entities;

namespace Infrastructure.Interfaces;

public interface ITaskEvent
{
    public Task<TaskModel> AddUserTask(string username);
    public Task<TaskModel?> ChangeUserTask(int id);
}