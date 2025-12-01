namespace Infrastructure.Interfaces;

public interface IStorageService
{
    Task<List<string>> GetTemporaryUrlsAsync(List<string> fileNames);
}