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
using AssetManagement.Api.Middleware;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("BlazorPolicy", policy =>
    {
        // Allow the React dev origin (exact scheme+host+port)
        policy.WithOrigins("http://localhost:8080", "https://localhost:8080")
              .AllowAnyHeader()
              .AllowAnyMethod()
              // If your client sends cookies or credentials, enable this.
              // If not using credentials, you can remove AllowCredentials().
              .AllowCredentials();
    });

    // Optional permissive policy for local debugging (use only in Development)
    options.AddPolicy("DevAllowAll", policy =>
    {
        policy.SetIsOriginAllowed(_ => true)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

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
builder.Services.AddScoped<IAppDbContext>(provider => provider.GetRequiredService<AppDbContext>());
builder.Services.AddScoped<IIdentityService, IdentityService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IAssetRepository, AssetRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IAssetAssignmentReportRepository, AssetAssignmentReportRepository>();

// MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(
    typeof(Program).Assembly,
    typeof(AssetManagement.Application.DependencyInjection).Assembly,
    typeof(AppDbContext).Assembly
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
        var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        await SeedPermissions(context);
        await SeedAdminUser(userManager, roleManager, context, builder.Configuration);
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

app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseHttpsRedirection();
app.UseCors("BlazorPolicy");
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

// Helper method to seed default admin user
static async Task SeedAdminUser(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, AppDbContext context, IConfiguration configuration)
{
    var adminEmail = configuration["SeedData:AdminEmail"] ?? "admin@assetmanagement.local";
    var adminUsername = configuration["SeedData:AdminUsername"] ?? "admin";
    var adminPassword = configuration["SeedData:AdminPassword"] ?? "AdminPassword123!";

    var user = await userManager.FindByNameAsync(adminUsername);
    if (user == null)
    {
        user = new IdentityUser
        {
            UserName = adminUsername,
            Email = adminEmail,
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(user, adminPassword);
        if (result.Succeeded)
        {
            var adminRoleName = "Admin";

            if (!await roleManager.RoleExistsAsync(adminRoleName))
            {
                var adminRole = new IdentityRole(adminRoleName);
                await roleManager.CreateAsync(adminRole);

                // Assign ALL permissions to the Admin role
                var allPermissions = await context.Permissions.ToListAsync();
                var rolePermissions = allPermissions.Select(p => new AssetManagement.Domain.Entities.RolePermission(
                    adminRole.Id,
                    p.Id
                ));

                await context.RolePermissions.AddRangeAsync(rolePermissions);
                await context.SaveChangesAsync();
            }

            // Assign user to Admin role
            await userManager.AddToRoleAsync(user, adminRoleName);
        }
    }
}