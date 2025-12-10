using Infrastructure.Interfaces;
using Shared.Models.Entities;
using Shared.Models.Enums;

namespace Infrastructure.Mocks;

public class MetricsTableMock :IMetricsTable
{
    public Task<bool> AddRecord(string username, MetricType type)
    {
        throw new NotImplementedException();
    }

    public Task<List<MetricModel>> ReadAllRecord()
    {
        throw new NotImplementedException();
    }
}