using System;

namespace webapi.Models
{
    public class TodoItem
    {
        public int Id { get; set; }
        public string UserId { get; set; } = default!;
        
        public required string Title { get; set; }
        public string? Content { get; set; }
        
        public bool Completed { get; set; }
        public DateTime? CompletedOn { get; set; }
        
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public DateTime? UpdatedOn { get; set; }
        
        public bool Deleted { get; set; }
        
        public List<FileForNote> Files { get; set; } = new();
    }
}