using AssetManagement.Application.DTOs.Auth;
using AssetManagement.Application.Services;
using AssetManagement.Application.UseCases.Auth;
using AssetManagement.Domain.Entities;
using AssetManagement.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AssetManagement.Application.Handlers.Auth;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthResponseDto>
{
    private readonly IIdentityService _identityService;
    private readonly ITokenService _tokenService;
    private readonly IAppDbContext _context;

    public RegisterCommandHandler(IIdentityService identityService, ITokenService tokenService, IAppDbContext context)
    {
        _identityService = identityService;
        _tokenService = tokenService;
        _context = context;
    }

    public async Task<AuthResponseDto> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var dto = request.RegisterDto;

        var (succeeded, userId, errors) = await _identityService.RegisterAsync(dto);
        if (!succeeded)
        {
            throw new InvalidOperationException($"Failed to create user: {string.Join(", ", errors)}");
        }

        // Get user info (to get the confirmed username/email for token)
        var userInfo = await _identityService.GetUserByIdAsync(userId);
        if (userInfo == null) throw new InvalidOperationException("User created but not found");

        // New accounts have no permissions initially
        var permissions = new List<string>();

        // Generate tokens
        var token = _tokenService.GenerateAccessToken(userId, userInfo.Username, userInfo.Email, permissions);
        var refreshTokenStr = _tokenService.GenerateRefreshToken();

        // Store refresh token
        var refreshToken = new RefreshToken(refreshTokenStr, userId);
        await _context.RefreshTokens.AddAsync(refreshToken, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return new AuthResponseDto(
            token,
            refreshTokenStr,
            DateTime.UtcNow.AddHours(1),
            new UserInfoDto(
                userId,
                userInfo.Username,
                userInfo.Email,
                dto.FirstName,
                dto.LastName,
                permissions
            )
        );
    }
}

public class LoginQueryHandler : IRequestHandler<LoginQuery, AuthResponseDto>
{
    private readonly IIdentityService _identityService;
    private readonly ITokenService _tokenService;
    private readonly IAppDbContext _context;

    public LoginQueryHandler(
        IIdentityService identityService,
        ITokenService tokenService,
        IAppDbContext context)
    {
        _identityService = identityService;
        _tokenService = tokenService;
        _context = context;
    }

    public async Task<AuthResponseDto> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        var dto = request.LoginDto;

        var (succeeded, userId, errors) = await _identityService.CheckPasswordAsync(dto.Username, dto.Password);
        if (!succeeded)
            throw new UnauthorizedAccessException("Invalid username or password");

        var userInfo = await _identityService.GetUserByIdAsync(userId);
        if (userInfo == null)
            throw new UnauthorizedAccessException("User not found");

        // Generate tokens
        var token = _tokenService.GenerateAccessToken(userId, userInfo.Username, userInfo.Email, userInfo.Permissions);
        var refreshTokenStr = _tokenService.GenerateRefreshToken();

        // Store refresh token
        var refreshToken = new RefreshToken(refreshTokenStr, userId);
        await _context.RefreshTokens.AddAsync(refreshToken, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return new AuthResponseDto(
            token,
            refreshTokenStr,
            DateTime.UtcNow.AddHours(1),
            userInfo
        );
    }
}

public class GetCurrentUserQueryHandler : IRequestHandler<GetCurrentUserQuery, UserInfoDto>
{
    private readonly IIdentityService _identityService;

    public GetCurrentUserQueryHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<UserInfoDto> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
    {
        var userInfo = await _identityService.GetUserByIdAsync(request.UserId);
        if (userInfo == null)
            throw new KeyNotFoundException("User not found");

        return userInfo;
    }
}
