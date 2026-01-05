using AssetManagement.Application.DTOs.Auth;
using AssetManagement.Application.Services;
using AssetManagement.Application.UseCases.Auth;
using AssetManagement.Domain.Entities;
using AssetManagement.Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AssetManagement.Infrastructure.Handlers.Auth;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthResponseDto>
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ITokenService _tokenService;
    private readonly AppDbContext _context;

    public RegisterCommandHandler(UserManager<IdentityUser> userManager, ITokenService tokenService, AppDbContext context)
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _context = context;
    }

    public async Task<AuthResponseDto> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var dto = request.RegisterDto;

        // Check if user already exists
        var existingUser = await _userManager.FindByNameAsync(dto.Username);
        if (existingUser != null)
            throw new InvalidOperationException("Username already exists");

        var existingEmail = await _userManager.FindByEmailAsync(dto.Email);
        if (existingEmail != null)
            throw new InvalidOperationException("Email already exists");

        // Create new user
        var user = new IdentityUser
        {
            UserName = dto.Username,
            Email = dto.Email,
            EmailConfirmed = true // For simplicity, auto-confirm
        };

        var result = await _userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new InvalidOperationException($"Failed to create user: {errors}");
        }

        // Get user permissions (empty for new user)
        var permissions = new List<string>();

        // Generate tokens
        var token = _tokenService.GenerateAccessToken(user.Id, user.UserName ?? string.Empty, user.Email ?? string.Empty, permissions);
        var refreshToken = _tokenService.GenerateRefreshToken();

        return new AuthResponseDto(
            token,
            refreshToken,
            DateTime.UtcNow.AddHours(1),
            new UserInfoDto(
                user.Id,
                user.UserName ?? string.Empty,
                user.Email ?? string.Empty,
                dto.FirstName,
                dto.LastName,
                permissions
            )
        );
    }
}

public class LoginQueryHandler : IRequestHandler<LoginQuery, AuthResponseDto>
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly ITokenService _tokenService;
    private readonly AppDbContext _context;

    public LoginQueryHandler(
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        ITokenService tokenService,
        AppDbContext context)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
        _context = context;
    }

    public async Task<AuthResponseDto> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        var dto = request.LoginDto;

        var user = await _userManager.FindByNameAsync(dto.Username);
        if (user == null)
            throw new UnauthorizedAccessException("Invalid username or password");

        var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, lockoutOnFailure: false);
        if (!result.Succeeded)
            throw new UnauthorizedAccessException("Invalid username or password");

        // Get user permissions
        var permissions = await _context.UserPermissions
            .Where(up => up.UserId == user.Id)
            .Include(up => up.Permission)
            .Where(up => up.Permission.IsActive)
            .Select(up => up.Permission.Name)
            .ToListAsync(cancellationToken);

        // Generate tokens
        var token = _tokenService.GenerateAccessToken(user.Id, user.UserName ?? string.Empty, user.Email ?? string.Empty, permissions);
        var refreshToken = _tokenService.GenerateRefreshToken();

        return new AuthResponseDto(
            token,
            refreshToken,
            DateTime.UtcNow.AddHours(1),
            new UserInfoDto(
                user.Id,
                user.UserName ?? string.Empty,
                user.Email ?? string.Empty,
                string.Empty, // FirstName not stored in IdentityUser
                string.Empty, // LastName not stored in IdentityUser
                permissions
            )
        );
    }
}

public class GetCurrentUserQueryHandler : IRequestHandler<GetCurrentUserQuery, UserInfoDto>
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly AppDbContext _context;

    public GetCurrentUserQueryHandler(UserManager<IdentityUser> userManager, AppDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    public async Task<UserInfoDto> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId);
        if (user == null)
            throw new KeyNotFoundException("User not found");

        // Get user permissions
        var permissions = await _context.UserPermissions
            .Where(up => up.UserId == request.UserId)
            .Include(up => up.Permission)
            .Where(up => up.Permission.IsActive)
            .Select(up => up.Permission.Name)
            .ToListAsync(cancellationToken);

        return new UserInfoDto(
            user.Id,
            user.UserName ?? string.Empty,
            user.Email ?? string.Empty,
            string.Empty, // FirstName
            string.Empty, // LastName
            permissions
        );
    }
}
