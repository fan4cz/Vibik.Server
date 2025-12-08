using Api.Application.Features.Photos.UploadPhoto;
using MediatR;

namespace Api.Application.Features.Tasks.SubmitTask;

public record SubmitTaskQuery(string Username, int TaskId, List<IFormFile> Files) : IRequest<List<string>>;