namespace Shared.Models.Entities;

public class TaskModelExtendedInfoExtension
{
    public required string Description { get; set; }
    public int PhotosRequired { get; set; }
    public required string[] ExamplePhotos { get; set; }
    public required string[] UserPhotos { get; set; }

    public TaskModelExtendedInfo ToTaskModelExtendedInfo()
    {
        // var examplePhotos = ExamplePhotos.Select(x => new Uri("")).ToList();
        // var userPhotos = UserPhotos.Select(x => new Uri("")).ToList();

        return new TaskModelExtendedInfo
        {
            Description = Description,
            PhotosRequired = PhotosRequired,
            ExamplePhotos = [],
            UserPhotos = []
        };
    }
}