namespace webapi.Services.AWS;

public interface IAWSS3Service
{
    Task<string> UploadFile(IFormFile file, string userId);
}