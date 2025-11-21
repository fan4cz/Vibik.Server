namespace Shared.Models;

public class User
{
    public required string Username { get; set; }
    public required string DisplayName { get; set; }
    public int Level { get; set; } = 0;
    public int Experience { get; set; } = 0;
    public int Money { get; set; } = 0;
}