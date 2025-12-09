using Shared.Models.Enums;

namespace Shared.Models.Entities;

public class MetricModel
{
    public int Id { get; set; }
    public string Username { get; set; }
    public MetricType Type { get; set; }
    public DateTime Time { get; set; }
}