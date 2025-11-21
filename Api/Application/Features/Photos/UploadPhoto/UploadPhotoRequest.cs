namespace Api.Application.Features.Photos.UploadPhoto;

public class UploadPhotoRequest
{
    public required IFormFile File { get; set; }
}