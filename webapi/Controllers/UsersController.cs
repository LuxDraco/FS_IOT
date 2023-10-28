using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webapi.Models;

namespace webapi.Controllers;

[ApiController]
[Route("v1/[controller]")]
public class UsersController: ControllerBase
{
    private readonly ToDoDbContext _dbContext;
    
    public UsersController(ToDoDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetUsers()
    {
        var users = await _dbContext.Users.ToListAsync();
        return Ok(users);
    }
    
    // Registra un nuevo usuario
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> CreateUser(User user)
    {
        var result = await _dbContext.Users.AddAsync(user);
        await _dbContext.SaveChangesAsync();
        return CreatedAtAction(nameof(GetUsers), new { id = result.Entity.Id }, result.Entity);
    }
}