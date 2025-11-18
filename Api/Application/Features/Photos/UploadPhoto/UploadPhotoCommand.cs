using MediatR;

namespace Api.Application.Features.Photos.UploadPhoto;

public record UploadPhotoCommand(IFormFile File) : IRequest<string>;