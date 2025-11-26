namespace Shared.Models.Entities;

public class TaskModelExtendedInfoExtension
{
    public required string Description { get; set; }
    public int PhotosRequired { get; set; }
    public required string[]? ExamplePhotos { get; set; }
    public required string[]? UserPhotos { get; set; }

    public TaskModelExtendedInfo ToTaskModelExtendedInfo()
    {
        return new TaskModelExtendedInfo
        {
            Description = Description,
            PhotosRequired = PhotosRequired,
            ExamplePhotos = ExamplePhotos is null ? [] : ExamplePhotos.ToList(),
            UserPhotos = UserPhotos is null ? [] : UserPhotos.ToList()
        };
    }
}