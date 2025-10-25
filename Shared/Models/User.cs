namespace Shared.Models;

public class User
{
    public string Username { get; set; }
    public string DisplayName { get; set; }
    public int Level { get; set; } = 0;
    public int Experience { get; set; } = 0;
}