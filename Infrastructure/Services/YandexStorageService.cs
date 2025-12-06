using Amazon.S3;
using Amazon.S3.Model;
using Infrastructure.Interfaces;
using Microsoft.Extensions.Options;
using Shared.Models.Configs;

namespace Infrastructure.Services;

public class YandexStorageService(IAmazonS3 s3Client, IOptions<YosConfig> cfg) : IStorageService
{
    private readonly string bucketName = cfg.Value.BucketName;

    public async Task<List<Uri>> GetTemporaryUrlsAsync(List<string> fileNames)
    {
        var tasks = fileNames.Select(GenerateTemporaryUrl).ToList();
        return (await Task.WhenAll(tasks)).ToList();
    }

    private Task<Uri> GenerateTemporaryUrl(string fileName)
    {
        var req = new GetPreSignedUrlRequest
        {
            BucketName = bucketName,
            Key = fileName,
            Verb = HttpVerb.GET,
            Expires = DateTime.Now.AddHours(1)
        };
        
        var url = s3Client.GetPreSignedURL(req);
        return Task.FromResult(new Uri(url));
    }
}