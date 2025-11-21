namespace Shared.Models;

public class TaskModel
{
    public int UserTaskId { get; set; }
    public required string TaskId { get; set; }
    public required string Name { get; set; }
    public DateTime StartTime { get; set; }
    public int Reward { get; set; }
    public TaskModelExtendedInfo? ExtendedInfo { get; set; }

    private const int MagicConst = 1;
    public int Swap => Reward * MagicConst;
}