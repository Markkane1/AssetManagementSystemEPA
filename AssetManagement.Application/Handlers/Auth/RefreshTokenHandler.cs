using AssetManagement.Application.DTOs.Auth;
using AssetManagement.Application.Interfaces;
using AssetManagement.Application.Services;
using AssetManagement.Application.UseCases.Auth;
using AssetManagement.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AssetManagement.Application.Handlers.Auth;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, AuthResponseDto>
{
    private readonly ITokenService _tokenService;
    private readonly IAppDbContext _context;
    private readonly IIdentityService _identityService;

    public RefreshTokenCommandHandler(ITokenService tokenService, IAppDbContext context, IIdentityService identityService)
    {
        _tokenService = tokenService;
        _context = context;
        _identityService = identityService;
    }

    public async Task<AuthResponseDto> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var principal = _tokenService.GetPrincipalFromExpiredToken(request.AccessToken);
        if (principal == null)
            throw new UnauthorizedAccessException("Invalid access token");

        var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
            throw new UnauthorizedAccessException("Invalid access token claims");

        var savedRefreshToken = await _context.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.Token == request.RefreshToken && rt.UserId == userId, cancellationToken);

        if (savedRefreshToken == null || !savedRefreshToken.IsActive)
            throw new UnauthorizedAccessException("Invalid or expired refresh token");

        var userInfo = await _identityService.GetUserByIdAsync(userId);
        if (userInfo == null)
            throw new UnauthorizedAccessException("User not found");

        // Mark old token as used
        savedRefreshToken.Use();
        _context.RefreshTokens.Update(savedRefreshToken);

        // Generate new tokens
        var newAccessToken = _tokenService.GenerateAccessToken(userId, userInfo.Username, userInfo.Email, userInfo.Permissions);
        var newRefreshTokenStr = _tokenService.GenerateRefreshToken();

        // Store new refresh token with tracking info
        var newRefreshToken = new RefreshToken(newRefreshTokenStr, userId, request.DeviceInfo, request.IPAddress);
        await _context.RefreshTokens.AddAsync(newRefreshToken, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return new AuthResponseDto(
            newAccessToken,
            newRefreshTokenStr,
            DateTime.UtcNow.AddHours(1),
            userInfo
        );
    }
}
