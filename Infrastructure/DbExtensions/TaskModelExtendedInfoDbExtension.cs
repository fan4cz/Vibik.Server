using Infrastructure.Interfaces;
using Shared.Models.Entities;

namespace Infrastructure.DbExtensions;

public abstract class TaskModelExtendedInfoDbExtension(IStorageService storageService)
{
    public required string Description { get; set; }
    public int PhotosRequired { get; set; }
    public required string[] ExamplePhotos { get; set; }
    public required string[] UserPhotos { get; set; }

    public async Task<TaskModelExtendedInfo> ToTaskModelExtendedInfo()
    {
        var examplePhotos = await storageService.GetTemporaryUrlsAsync(this.ExamplePhotos.ToList());
        var userPhotos = await storageService.GetTemporaryUrlsAsync(this.UserPhotos.ToList());

        return new TaskModelExtendedInfo
        {
            Description = this.Description,
            PhotosRequired = this.PhotosRequired,
            ExamplePhotos = examplePhotos,
            UserPhotos = userPhotos
        };
    }
}