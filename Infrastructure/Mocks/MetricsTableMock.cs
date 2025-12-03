using Infrastructure.Interfaces;
using Shared.Models.Entities;
using Shared.Models.Enums;

namespace Infrastructure.Mocks;

public class MetricsTableMock :IMetricsTable
{
    public bool AddRecord(MetricType type)
    {
        throw new NotImplementedException();
    }

    public List<MetricModel> ReadAllRecord()
    {
        throw new NotImplementedException();
    }
}