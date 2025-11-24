using MediatR;

namespace Api.Application.Features.Tasks.SubmitTask;

public record SubmitTaskQuery(string Username, string TaskId, List<IFormFile> Files) :  IRequest<List<string>>;