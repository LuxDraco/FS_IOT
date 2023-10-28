using Microsoft.AspNetCore.Identity;

namespace webapi.Models;

public class User : IdentityUser
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public DateTime CreatedOn { get; set; } = DateTime.Now;
}