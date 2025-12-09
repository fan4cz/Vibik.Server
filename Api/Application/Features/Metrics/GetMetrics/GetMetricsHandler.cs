using Api.Application.Features.Moderation.ApproveTask;
using Infrastructure.Interfaces;
using MediatR;
using Shared.Models.Entities;
using Shared.Models.Enums;

namespace Api.Application.Features.Metrics.GetMetrics;

public class GetMetricsHandler(IMetricsTable metrics) : IRequestHandler<GetMetricsQuery, List<MetricModel>>
{
    public async Task<List<MetricModel>> Handle(GetMetricsQuery request, CancellationToken cancellationToken)
    {
        return await metrics.ReadAllRecord();
    }
}


