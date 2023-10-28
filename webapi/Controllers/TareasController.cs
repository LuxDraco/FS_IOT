using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webapi.Models;

namespace webapi.Controllers;

[ApiController]
[Route("v1/[controller]")]
[Authorize]
public class TareasController : ControllerBase
{
    private readonly ToDoDbContext _dbContext;
        
    public TareasController(ToDoDbContext dbContext)
    {
        _dbContext = dbContext;
    }
        
    [HttpGet]
    public async Task<IActionResult> GetTodoItems()
    {
        var userId = User.FindFirst(ClaimTypes.Sid)?.Value;
        var todoItems = await _dbContext.TodoItems
            .Where(x => x.UserId == userId)
            .Where(x => !x.Deleted)
            .Include(f => f.Files)
            .ToListAsync();
        return Ok(todoItems);
    }

    [HttpPost]
    public async Task<IActionResult> CreateItem(TodoItem todoItem)
    {
        todoItem.UserId = User.FindFirst(ClaimTypes.Sid)?.Value ?? throw new InvalidOperationException("User is not authenticated");
        
        _dbContext.Add(todoItem);
        await _dbContext.SaveChangesAsync();
        
        return CreatedAtAction(nameof(GetTodoItems), new { id = todoItem.Id }, todoItem);
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateItem(int id, TodoItem todoItem)
    {
        if (User.Identity is { IsAuthenticated: false })
        {
            return Unauthorized();
        }
    
        // Verificar que el ID del item enviado coincide con el ID en la ruta
        if (id != todoItem.Id)
        {
            return BadRequest();
        }

        var userId = User.FindFirst(ClaimTypes.Sid)?.Value;
        if (todoItem.UserId != userId)
        {
            return Forbid("You do not have permission to update this item.");
        }

        // Marcar el item como modificado
        _dbContext.Entry(todoItem).State = EntityState.Modified;

        try
        {
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_dbContext.TodoItems.Any(e => e.Id == id))
            {
                return NotFound();
            }

            throw;
        }
    
        return NoContent();
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteItem(int id)
    {
        if (User.Identity is { IsAuthenticated: false })
        {
            return Unauthorized();
        }

        var userId = User.FindFirst(ClaimTypes.Sid)?.Value;

        // Buscar el item por ID y asegurarse de que pertenece al usuario actual
        var todoItem = await _dbContext.TodoItems.FirstOrDefaultAsync(e => e.Id == id && e.UserId == userId);
    
        if (todoItem == null)
        {
            return NotFound();
        }

        todoItem.Deleted = true;
        await _dbContext.SaveChangesAsync();
    
        return NoContent();
    }
}