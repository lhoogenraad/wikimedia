using Infrastructure.Database;  // Ensure MongoDbContext is found
using Microsoft.Extensions.Options;
using Application.Services.Auth;
using Microsoft.IdentityModel.Tokens;
using Infrastructure.Log;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

// Inject mongo DB context
var mongoDbSettings = builder.Configuration.GetSection("MongoDbSettings").Get<MongoDbSettings>();
builder.Services.AddSingleton(mongoDbSettings);
builder.Services.AddSingleton<IConsoleLogger, ConsoleLogger>();
builder.Services.AddSingleton<MongoDbContext>();

builder.Services.AddControllers();
builder.Services.AddSingleton<IAuthService, AuthService>();

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(options =>
    {
        var config = builder.Configuration;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Secret"])),
            ValidateIssuer = true,
            ValidIssuer = config["Jwt:Issuer"],
            ValidateAudience = true,
            ValidAudience = config["Jwt:Audience"],
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

