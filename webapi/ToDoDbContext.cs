using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using webapi.Models;

namespace webapi;

public class ToDoDbContext: IdentityDbContext<User>
{
    private readonly ILoggerFactory _loggerFactory = new LoggerFactory();
    public ToDoDbContext(DbContextOptions<ToDoDbContext> options) : base(options)
    {
        
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLoggerFactory(_loggerFactory);
        base.OnConfiguring(optionsBuilder);
    }
    
    public static readonly ILoggerFactory MyLoggerFactory = LoggerFactory.Create(builder =>
    {
        builder.AddFilter((category, level) =>
            category == DbLoggerCategory.Database.Command.Name
            && level == LogLevel.Information)
            .AddConsole();
    });

    public DbSet<TodoItem> TodoItems { get; set; } = default!;
    public DbSet<FileForNote> FilesForNotes { get; set; } = default!;
}