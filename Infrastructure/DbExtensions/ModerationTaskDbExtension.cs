using Shared.Models.Entities;

namespace Infrastructure.DbExtensions;

public class ModerationTaskDbExtension
{
    public int UserTaskId { get; set; }
    public required string TaskId { get; set; }
    public required string Name { get; set; }
    public required string[]? Tags { get; set; }

    public async Task<ModerationTask> ToModerationTask()
    {
        return new ModerationTask()
        {
            UserTaskId = this.UserTaskId,
            TaskId = this.TaskId,
            Name = this.Name,
            Tags = Tags is null ? [] : Tags.ToList()
        };
    }
}