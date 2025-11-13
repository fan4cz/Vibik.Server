using MediatR;
using Minio;
using Minio.DataModel.Args;

namespace Api.Application.Features.Photos.UploadPhoto;

public class UploadPhotoHandler(IMinioClient minio, IConfiguration config) : IRequestHandler<UploadPhotoCommand, string>
{
    private readonly string bucket = config["Minio:Bucket"];

    public async Task<string> Handle(UploadPhotoCommand request, CancellationToken cancellationToken)
    {
        var file = request.File;

        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        var contentType = file.ContentType;

        var exists = await minio.BucketExistsAsync(new BucketExistsArgs().WithBucket(bucket),
            cancellationToken);
        if (!exists)
            await minio.MakeBucketAsync(new MakeBucketArgs().WithBucket(bucket), cancellationToken);

        await using var stream = file.OpenReadStream();

        await minio.PutObjectAsync(
            new PutObjectArgs()
                .WithBucket(bucket)
                .WithObject(fileName)
                .WithStreamData(stream)
                .WithObjectSize(file.Length)
                .WithContentType(contentType), cancellationToken);

        return fileName;
    }
}