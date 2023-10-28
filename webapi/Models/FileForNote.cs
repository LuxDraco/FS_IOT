namespace webapi.Models;

public class FileForNote
{
    public int Id { get; set; }
    public required string Path { get; set; }
    public required string Name { get; set; }
    public required string FileType { get; set; }
    public required long FileSize { get; set; }
    public required string FileHeaders { get; set; }
    
    public required int TodoItemId { get; set; }
    public TodoItem TodoItem { get; set; } = default!;
    
    public DateTime CreatedOn { get; set; } = DateTime.Now;
    public DateTime? UpdatedOn { get; set; }
}