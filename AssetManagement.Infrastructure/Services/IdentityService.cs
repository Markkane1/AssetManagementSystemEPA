using AssetManagement.Application.DTOs.Auth;
using AssetManagement.Application.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AssetManagement.Infrastructure.Services;

public class IdentityService : IIdentityService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IAppDbContext _context;

    public IdentityService(
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        RoleManager<IdentityRole> roleManager,
        IAppDbContext context)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _context = context;
    }

    public async Task<(bool Succeeded, string UserId, string[] Errors)> RegisterAsync(RegisterDto registerDto)
    {
        var user = new IdentityUser
        {
            UserName = registerDto.Username,
            Email = registerDto.Email,
            EmailConfirmed = true
        };

        var result = await _userManager.CreateAsync(user, registerDto.Password);

        if (result.Succeeded)
        {
            return (true, user.Id, Array.Empty<string>());
        }

        return (false, string.Empty, result.Errors.Select(e => e.Description).ToArray());
    }

    public async Task<(bool Succeeded, string UserId, string[] Errors)> CheckPasswordAsync(string username, string password)
    {
        var user = await _userManager.FindByNameAsync(username);
        if (user == null)
            return (false, string.Empty, new[] { "Invalid username or password" });

        var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);

        if (result.Succeeded)
        {
            return (true, user.Id, Array.Empty<string>());
        }

        return (false, string.Empty, new[] { "Invalid username or password" });
    }

    public async Task<UserInfoDto?> GetUserByIdAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return null;

        var permissions = await GetUserPermissionsAsync(user.Id);

        return new UserInfoDto(
            user.Id,
            user.UserName ?? string.Empty,
            user.Email ?? string.Empty,
            string.Empty, // FirstName (can be extended if needed)
            string.Empty, // LastName
            permissions
        );
    }

    public async Task<UserInfoDto?> GetUserByNameAsync(string username)
    {
        var user = await _userManager.FindByNameAsync(username);
        if (user == null) return null;

        var permissions = await GetUserPermissionsAsync(user.Id);

        return new UserInfoDto(
            user.Id,
            user.UserName ?? string.Empty,
            user.Email ?? string.Empty,
            string.Empty,
            string.Empty,
            permissions
        );
    }

    public async Task<List<int>> GetAllowedLocationIdsAsync(string userId)
    {
        return await _context.UserLocationAccess
            .Where(ula => ula.UserId == userId)
            .Select(ula => ula.LocationId)
            .ToListAsync();
    }

    public async Task<List<string>> GetRolesAsync()
    {
        return await _roleManager.Roles.Select(r => r.Name!).ToListAsync();
    }

    public async Task<bool> AddToRoleAsync(string userId, string roleName)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return false;

        var result = await _userManager.AddToRoleAsync(user, roleName);
        return result.Succeeded;
    }

    public async Task<bool> RemoveFromRoleAsync(string userId, string roleName)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return false;

        var result = await _userManager.RemoveFromRoleAsync(user, roleName);
        return result.Succeeded;
    }

    public async Task<IList<string>> GetUserRolesAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return new List<string>();

        return await _userManager.GetRolesAsync(user);
    }

    public async Task<List<string>> GetUserPermissionsAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return new List<string>();

        var userRoles = await _userManager.GetRolesAsync(user);

        var roleIds = await _roleManager.Roles
            .Where(r => userRoles.Contains(r.Name!))
            .Select(r => r.Id)
            .ToListAsync();

        return await _context.RolePermissions
            .Where(rp => roleIds.Contains(rp.RoleId))
            .Include(rp => rp.Permission)
            .Where(rp => rp.Permission.IsActive)
            .Select(rp => rp.Permission.Name)
            .Distinct()
            .ToListAsync();
    }
}
