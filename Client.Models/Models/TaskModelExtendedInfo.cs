namespace Shared.Models;

public class TaskModelExtendedInfo
{
    public required string Description { get; set; }
    public int PhotosRequired { get; set; }
    public required List<Uri> ExamplePhotos { get; set; }
    public required List<Uri> UserPhotos { get; set; }
}