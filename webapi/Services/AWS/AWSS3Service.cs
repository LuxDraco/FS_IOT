using Amazon.S3;
using Amazon.S3.Model;
using webapi.Models;

namespace webapi.Services.AWS;

public class AwsS3Service: IAWSS3Service
{
    private readonly IAmazonS3 _s3Client;
    
    public AwsS3Service(IAmazonS3 s3Client)
    {
        _s3Client = s3Client;
    }
    
    public async Task<string> UploadFile(IFormFile file, string userId)
    {
        // Get Unix timestamp
        var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        var location = $"uploads/{userId}/{file.FileName}_{timestamp}";
        
        // Get bucket name from appsettings.json
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
        
        var bucketName = config["AWS:BucketName"];
        Console.WriteLine($"bucketName: {bucketName}");

        await using var stream = file.OpenReadStream();
        var request = new PutObjectRequest
        {
            InputStream = stream,
            BucketName = bucketName,
            Key = location,
            ContentType = file.ContentType,
            AutoCloseStream = true
        };
        
        
        var response = await _s3Client.PutObjectAsync(request);
        return location;
    }
}