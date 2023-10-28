using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webapi.Models;

namespace webapi.Controllers;

[ApiController]
[Route("v1/[controller]")]
[Authorize]
public class FileForNoteController: ControllerBase
{
    private readonly ToDoDbContext _dbContext;
    
    public FileForNoteController(ToDoDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetFileForNote()
    {
        if (User.Identity is { IsAuthenticated: false })
        {
            return Unauthorized();
        }
        
        var userId = User.FindFirst(ClaimTypes.Sid)?.Value;
        var todoItems = await _dbContext.TodoItems.Where(x => x.UserId == userId).ToListAsync();
        return Ok(todoItems);
    }

    [HttpPost]
    public async Task<IActionResult> SaveFile(FileForNote fileForNote)
    {
        if (User.Identity is { IsAuthenticated: false })
        {
            return Unauthorized();
        }

        _dbContext.Add(fileForNote);
        await _dbContext.SaveChangesAsync();
        
        return CreatedAtAction(nameof(GetFileForNote), new { id = fileForNote.Id }, fileForNote);
    }
}