using Infrastructure.Interfaces;
using Shared.Models.Entities;

namespace Infrastructure.DbExtensions;

public class TaskModelExtendedInfoDbExtension
{
    public required string Description { get; set; }
    public int PhotosRequired { get; set; }
    public string[] ExamplePhotos { get; set; } = Array.Empty<string>();
    public string[] UserPhotos { get; set; } = Array.Empty<string>();

    public async Task<TaskModelExtendedInfo> ToTaskModelExtendedInfo(IStorageService storageService)
    {
        var examplePhotos = await storageService.GetTemporaryUrlsAsync((ExamplePhotos ?? Array.Empty<string>()).ToList());
        var userPhotos = await storageService.GetTemporaryUrlsAsync((UserPhotos ?? Array.Empty<string>()).ToList());

        return new TaskModelExtendedInfo
        {
            Description = this.Description,
            PhotosRequired = this.PhotosRequired,
            ExamplePhotos = examplePhotos,
            UserPhotos = userPhotos
        };
    }
}
