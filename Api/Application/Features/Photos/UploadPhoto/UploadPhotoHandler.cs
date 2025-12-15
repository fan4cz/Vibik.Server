using Amazon.S3;
using Amazon.S3.Model;
using ImageMagick;
using MediatR;
using Microsoft.Extensions.Options;
using Shared.Models.Configs;

namespace Api.Application.Features.Photos.UploadPhoto;

public class UploadPhotoHandler : IRequestHandler<UploadPhotoCommand, string>
{
    private readonly string bucket;
    private readonly IAmazonS3 s3Client;
    private readonly ILogger<UploadPhotoHandler> logger;

    public UploadPhotoHandler(IAmazonS3 s3Client, IOptions<YosConfig> config, ILogger<UploadPhotoHandler> logger)
    {
        this.s3Client = s3Client;
        this.logger = logger;
        bucket = config.Value.BucketName;
    }

    public async Task<string> Handle(UploadPhotoCommand request, CancellationToken cancellationToken)
    {
        var file = request.File;

        await using var inputStream = file.OpenReadStream();
        using var image = new MagickImage(inputStream);

        image.Quality = 75;
        image.Strip();

        await using var compressedStream = new MemoryStream();
        await image.WriteAsync(compressedStream, cancellationToken);
        compressedStream.Position = 0;

        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        const string contentType = "image/jpeg";

        var buckets = await s3Client.ListBucketsAsync(cancellationToken);
        if (buckets.Buckets.All(b => b.BucketName != bucket))
        {
            logger.LogWarning("Бакет {Bucket} не найден", bucket);
            await s3Client.PutBucketAsync(new PutBucketRequest { BucketName = bucket }, cancellationToken);
        }

        logger.LogInformation("Загрузка фотки {FileName} в бакет {Bucket}", fileName, bucket);

        await using var stream = file.OpenReadStream();

        var putRequest = new PutObjectRequest
        {
            BucketName = bucket,
            Key = fileName,
            InputStream = compressedStream,
            ContentType = contentType
        };

        await s3Client.PutObjectAsync(putRequest, cancellationToken);

        logger.LogInformation("Файл {FileName} успешно загружен в бакет {Bucket}", fileName, bucket);

        return fileName;
    }
}