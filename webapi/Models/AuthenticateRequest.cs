namespace webapi.Models;

public record struct AuthenticateRequest(string UserName, string Password);