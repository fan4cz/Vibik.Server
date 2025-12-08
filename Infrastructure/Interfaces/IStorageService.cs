namespace Infrastructure.Interfaces;

public interface IStorageService
{
    Task<List<Uri>> GetTemporaryUrlsAsync(List<string> fileNames);
}