using MediatR;
using Shared.Models.Entities;

namespace Api.Application.Features.Metrics.GetMetrics;

public record GetMetricsQuery() : IRequest<List<MetricModel>>;