namespace Shared.Models;

public class ModerationTask
{
    public int UserTaskId { get; set; }
    public string TaskId { get; set; }
    public string Name { get; set; }
    public List<string> Tags { get; set; }
    public TaskModelExtendedInfo? ExtendedInfo { get; set; }

}