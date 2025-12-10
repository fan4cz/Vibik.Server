namespace Shared.Models.Entities;

public class TaskModel
{
    public int UserTaskId { get; set; }
    public string TaskId { get; set; }
    public string Name { get; set; }
    public DateTime StartTime { get; set; }
    public int Reward { get; set; }
    public TaskModelExtendedInfo ExtendedInfo { get; set; }
    
    
    private const double Coefficient = 0.2;
    public int Swap => (int)(Reward * Coefficient);
}