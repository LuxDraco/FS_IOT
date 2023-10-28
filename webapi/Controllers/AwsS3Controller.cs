using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Json;
using webapi.Models;
using webapi.Services.AWS;

namespace webapi.Controllers;

[ApiController]
[Route("v1/[controller]")]
[Authorize]
public class AwsS3Controller: ControllerBase
{
    private readonly IAWSS3Service _awsS3Service;
    private readonly ToDoDbContext _dbContext;
    
    public AwsS3Controller(IAWSS3Service awsS3Service, ToDoDbContext dbContext)
    {
        _awsS3Service = awsS3Service;
        _dbContext = dbContext;
    }
    
    [HttpPost("upload")]
    public async Task<IActionResult> Upload([FromForm] int todoItemId, IFormFile file)
    {
        // Log todoItemId and file.FileName
        Console.WriteLine($"todoItemId: {todoItemId} - file.FileName: {file.FileName}");
        
        var userSid = User.FindFirst(ClaimTypes.Sid)?.Value;
        if (string.IsNullOrEmpty(userSid))
        {
            return Forbid("User is not authenticated");
        }
        
        // Check if Note exists
        var todoItem = await _dbContext.TodoItems.FindAsync(todoItemId);
        if (todoItem == null)
        {
            return NotFound("Note not found");
        }
        
        // Max file size 2MB
        if (file.Length > 2 * 1024 * 1024)
        {
            return Ok($"File size is too big, max file size is 2MB {file.Length} - {file.FileName}");
        }
        
        var result = await _awsS3Service.UploadFile(file, userSid);
        
        // IHeaderDictionary map all key/value to string joined by comma
        var headers = file.Headers.ToDictionary(x => x.Key, x => x.Value.ToString());
        var stringHeaders = string.Join(";", headers.Select(x => $"{x.Key}:{x.Value}"));
        
        // Save file to database
        var fileForNote = new FileForNote
        {
            Path = result,
            Name = file.FileName,
            TodoItemId = todoItemId,
            FileType = file.ContentType,
            FileHeaders = stringHeaders,
            FileSize = file.Length
        };
        
        _dbContext.Add(fileForNote);
        await _dbContext.SaveChangesAsync();
        
        return CreatedAtAction(nameof(Upload), new { id = fileForNote.Id }, fileForNote);
    }
}