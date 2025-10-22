namespace Shared.Models;

public class TaskExtendedInfo
{
    public string Description { get; set; }
    public int PhotosRequired { get; set; }
    public List<PhotoModel> ExamplePhotos { get; set; }
    public List<PhotoModel> UserPhotos { get; set; }
}