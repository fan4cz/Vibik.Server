using Amazon.S3;
using Amazon.S3.Model;
using MediatR;

namespace Api.Application.Features.Photos.UploadPhoto;

public class UploadPhotoHandler(IAmazonS3 s3Client, IConfiguration config, ILogger<UploadPhotoHandler> logger)
    : IRequestHandler<UploadPhotoCommand, string>
{
    private readonly string bucket = config["YOS_BUCKET"]!;

    public async Task<string> Handle(UploadPhotoCommand request, CancellationToken cancellationToken)
    {
        var file = request.File;

        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        var contentType = file.ContentType;

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
            InputStream = stream,
            ContentType = contentType
        };

        await s3Client.PutObjectAsync(putRequest, cancellationToken);

        logger.LogInformation("Файл {FileName} успешно загружен в бакет {Bucket}", fileName, bucket);

        return fileName;
    }
}