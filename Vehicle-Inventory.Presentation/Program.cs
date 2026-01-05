using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Vehicle_Inventory.API.Middlewares;
using Vehicle_Inventory.Application.DependencyInjection;
using Vehicle_Inventory.Domain.Entities;
using Vehicle_Inventory.Infrastructure.Data;
using Vehicle_Inventory.Infrastructure.DependencyInjection;
using Vehicle_Inventory.Infrastructure.Settings;

var builder = WebApplication.CreateBuilder(args);

// ---------------- DATABASE ----------------
var connectionString = builder.Configuration.GetConnectionString("MyAppDbContext");
if (string.IsNullOrEmpty(connectionString))
{
    throw new Exception("Connection string is null! Check appsettings.json and environment.");
}
Console.WriteLine($"Using connection string: {connectionString}");

builder.Services.AddDbContext<MyAppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MyAppDbContext"))
);

// ---------------- FRAMEWORK SERVICES ----------------
builder.Services.AddControllers();

// ---------------- SWAGGER ----------------
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new() { Title = "Vehicle Inventory API", Version = "v1" });

    // Add JWT auth to Swagger
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

//// ---------------- APPLICATION & INFRASTRUCTURE ----------------
builder.Services
    .AddApplicationServices()
    .AddInfrastructureServices(builder.Configuration);

// ---------------- IDENTITY ----------------
//builder.Services.AddIdentity<User, IdentityRole>(options =>
//{
//    options.Password.RequireDigit = true;
//    options.Password.RequireUppercase = false;
//    options.Password.RequireLowercase = false;
//    options.Password.RequireNonAlphanumeric = false;
//    options.Password.RequiredLength = 6;
//})
//.AddEntityFrameworkStores<MyAppDbContext>()
//.AddDefaultTokenProviders();

// Register PasswordHasher explicitly (optional, DI will handle it)
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

builder.Services.Configure<CloudinarySettings>(
    builder.Configuration.GetSection("Cloudinary"));


// ---------------- JWT AUTHENTICATION ----------------
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings.GetValue<string>("Secret") ?? "SuperSecretKey123!VeryLongKeyyyyyy";

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings.GetValue<string>("Issuer") ?? "VehicleInventoryAPI",
        ValidAudience = jwtSettings.GetValue<string>("Audience") ?? "VehicleInventoryClient",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
    };
});

// ---------------- CORS ----------------
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .WithOrigins(
                "http://localhost:4200",   // Angular dev
                "http://localhost:5173"    // Vite (optional)
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials(); // agar cookies / auth headers bhej rahe ho
    });
});


// ---------------- BUILD APP ----------------
var app = builder.Build();

// ---------------- MIDDLEWARE ----------------
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Vehicle Inventory API V1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseMiddleware<UseExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseCors("AllowFrontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapGet("/", () => "Vehicle Inventory API running");

app.Run();