namespace Shared.Models;

public class TaskModelExtendedInfo
{
    public required string Description { get; set; }
    public int PhotosRequired { get; set; }
    public required List<string> ExamplePhotos { get; set; }
    public required List<string> UserPhotos { get; set; }
}