using Infrastructure.Interfaces;

public sealed class FakePasswordHasher : IPasswordHasher
{
    public string Hash(string password) => $"HASH::{password}";

    public bool Verify(string password, string hash) =>
        hash == $"HASH::{password}";
}