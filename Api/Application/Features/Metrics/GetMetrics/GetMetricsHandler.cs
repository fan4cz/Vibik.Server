using Infrastructure.Interfaces;
using MediatR;
using Shared.Models.Entities;

namespace Api.Application.Features.Metrics.GetMetrics;

public class GetMetricsHandler(IMetricsTable metrics) : IRequestHandler<GetMetricsQuery, List<MetricModel>>
{
    public async Task<List<MetricModel>> Handle(GetMetricsQuery request, CancellationToken cancellationToken)
    {
        return await metrics.ReadAllRecord();
    }
}


