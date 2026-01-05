using AssetManagement.Api.Authorization;
using AssetManagement.Api.Constants;
using AssetManagement.Application.Interfaces;
using AssetManagement.Application.Services;
using AssetManagement.Infrastructure;
using AssetManagement.Infrastructure.Data;
using AssetManagement.Infrastructure.Repositories;
using AssetManagement.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Swagger with JWT support
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Asset Management API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 8;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

// Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key not configured"))),
        ClockSkew = TimeSpan.Zero
    };
});

// Authorization with dynamic policies
builder.Services.AddAuthorization(options =>
{
    // Register all permission-based policies
    foreach (var permission in Permissions.GetAll())
    {
        options.AddPolicy(permission, policy =>
            policy.Requirements.Add(new PermissionRequirement(permission)));
    }
});

// Register authorization handler
builder.Services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();

// Application Services
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IAssetRepository, AssetRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IAssetAssignmentReportRepository, AssetAssignmentReportRepository>();

// MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(
    typeof(Program).Assembly,
    typeof(AssetManagement.Application.UseCases.Asset.GetAssetsByLocationQuery).Assembly,
    typeof(AssetManagement.Application.UseCases.Auth.LoginQuery).Assembly
));

// AutoMapper
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<AssetManagement.Application.Mappings.MappingProfile>();
}, typeof(Program).Assembly, typeof(AssetManagement.Application.Mappings.MappingProfile).Assembly);

var app = builder.Build();

// Seed permissions on startup
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        await SeedPermissions(context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding permissions");
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

// Helper method to seed permissions
static async Task SeedPermissions(AppDbContext context)
{
    if (await context.Permissions.AnyAsync())
        return; // Permissions already seeded

    var permissionsToSeed = new List<AssetManagement.Domain.Entities.Permission>();
    var allPermissions = Permissions.GetAll();

    foreach (var permissionName in allPermissions)
    {
        var category = permissionName.Split('.')[0];
        var action = permissionName.Split('.').Length > 1 ? permissionName.Split('.')[1] : "Unknown";

        var permission = new AssetManagement.Domain.Entities.Permission(
            permissionName,
            $"{action} permission for {category}",
            category
        );

        permissionsToSeed.Add(permission);
    }

    await context.Permissions.AddRangeAsync(permissionsToSeed);
    await context.SaveChangesAsync();
}