using Shared.Models.Entities;
using Shared.Models.Enums;

namespace Infrastructure.Interfaces;

public interface IMetricsTable
{
    bool AddRecord(MetricType type);
    List<MetricModel> ReadAllRecord();
}