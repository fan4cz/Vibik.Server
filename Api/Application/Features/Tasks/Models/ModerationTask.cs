namespace Shared.Models;

public class ModerationTask
{
    public int UserTaskId { get; set; }
    public string TaskId { get; set; }
    public string Name { get; set; }
    public List<TagsEnum> Tags { get; set; }
    public DateTime StartTime { get; set; }
    public int Reward { get; set; }
    public TaskExtendedInfo? ExtendedInfo { get; set; }

    private const int MagicConst = 1;
    public int Swap => Reward * MagicConst;
}