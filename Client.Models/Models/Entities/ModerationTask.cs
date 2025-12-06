namespace Shared.Models.Entities;

public class ModerationTask
{
    public int UserTaskId { get; set; }
    public required string TaskId { get; set; }
    public required string Name { get; set; }
    public required List<string> Tags { get; set; }
    public TaskModelExtendedInfo? ExtendedInfo { get; set; }
}