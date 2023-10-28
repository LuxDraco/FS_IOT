using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Amazon.Extensions.NETCore.Setup;
using Amazon.S3;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using webapi;
using webapi.Models;
using webapi.Services.AWS;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddIdentityCore<User>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ToDoDbContext>();

builder.Services.AddDbContext<ToDoDbContext>(options =>
    options
        .UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
        .UseLoggerFactory(ToDoDbContext.MyLoggerFactory)
);

// CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builderTmp =>
    {
        builderTmp.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

// AWS
var awsOptions = builder.Configuration.GetAWSOptions();
builder.Services.AddDefaultAWSOptions(awsOptions);
builder.Services.AddScoped<IAWSS3Service, AwsS3Service>();
builder.Services.AddAWSService<IAmazonS3>();

// Authentication
builder.Services
    .AddHttpContextAccessor()
    .AddAuthorization()
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddCookie()
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? ""))
        };
    })
    .AddGoogle(options =>
    {
        options.ClientId = builder.Configuration["Google:ClientId"] ?? "";
        options.ClientSecret = builder.Configuration["Google:ClientSecret"] ?? "";
        options.SignInScheme = IdentityConstants.ExternalScheme;
        options.Scope.Add("profile");
        options.Scope.Add("email");
        options.SaveTokens = true;
    });

// App *****************************************************************************************************************
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Demo IoT API v1");
        c.RoutePrefix = "docs";
    });
}

app.MapControllers();

await SeedData();

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

app.MapGet("/", () => "Demo IoT API v1, use /docs for Swagger UI");

app.MapPost("/token", async (AuthenticateRequest request, UserManager<User> userManager) =>
{
    var user = await userManager.FindByNameAsync(request.UserName);

    if (user is null || !await userManager.CheckPasswordAsync(user, request.Password))
    {
        return Results.Forbid();
    }

    var roles = await userManager.GetRolesAsync(user);
    var claims = new List<Claim>
    {
        new(ClaimTypes.Sid, user.Id),
        new(ClaimTypes.Name, user.UserName ?? string.Empty),
        new(ClaimTypes.GivenName, $"{user.FirstName} {user.LastName}")
    };

    claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

    var securityKey =
        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? string.Empty));
    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
    var tokenDescriptor = new JwtSecurityToken(
        issuer: builder.Configuration["Jwt:Issuer"],
        audience: builder.Configuration["Jwt:Audience"],
        claims: claims,
        expires: DateTime.Now.AddMinutes(720),
        signingCredentials: credentials);

    var jwt = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);

    return Results.Ok(new
    {
        AccessToken = jwt
    });
});

app.MapGet("/me", (IHttpContextAccessor contextAccessor) =>
    {
        if (contextAccessor.HttpContext == null) return Results.Forbid();
        var user = contextAccessor.HttpContext.User;

        return Results.Ok(new
        {
            Claims = user.Claims.Select(s => new
            {
                s.Type,
                s.Value
            }).ToList(),
            id = user.FindFirst(ClaimTypes.Sid)?.Value,
            email = user.FindFirst(ClaimTypes.Email)?.Value,
            user.Identity?.Name,
            user.Identity?.IsAuthenticated,
            user.Identity?.AuthenticationType
        });

    })
    .RequireAuthorization();

app.Run();
return;

async Task SeedData()
{
    var scopeFactory = app!.Services.GetRequiredService<IServiceScopeFactory>();
    using var scope = scopeFactory.CreateScope();

    var context = scope.ServiceProvider.GetRequiredService<ToDoDbContext>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

    context.Database.EnsureCreated();

    if (!userManager.Users.Any())
    {
        logger.LogInformation("Creando usuario de prueba");

        var newUser = new User
        {
            Email = "admin@demo.com",
            FirstName = "Poncho",
            LastName = "Draco",
            UserName = "poncho.demo"
        };

        await userManager.CreateAsync(newUser, "P@ss.W0rd");
        await roleManager.CreateAsync(new IdentityRole
        {
            Name = "Admin"
        });

        await roleManager.CreateAsync(new IdentityRole
        {
            Name = "UserR"
        });

        await userManager.AddToRoleAsync(newUser, "Admin");
    }
}