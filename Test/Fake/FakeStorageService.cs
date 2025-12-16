using Infrastructure.Interfaces;

public sealed class FakeStorageService : IStorageService
{
    public Task<List<Uri>> GetTemporaryUrlsAsync(List<string> fileNames)
    {
        if (fileNames is null || fileNames.Count == 0)
            return Task.FromResult(new List<Uri>());

        var result = fileNames
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Select(x =>
            {
                var safe = x.TrimStart('/').Replace("\\", "/");
                return new Uri($"https://fake.local/{safe}", UriKind.Absolute);
            })
            .ToList();

        return Task.FromResult(result);
    }
}