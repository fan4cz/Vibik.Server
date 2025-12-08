using Shared.Models.Entities;
using Shared.Models.Enums;

namespace Infrastructure.Interfaces;

public interface IMetricsTable
{
    Task<bool> AddRecord(string username, MetricType type);
    Task<List<MetricModel>> ReadAllRecord();
}