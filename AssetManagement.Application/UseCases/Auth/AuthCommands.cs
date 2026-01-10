using AssetManagement.Application.DTOs.Auth;
using MediatR;

namespace AssetManagement.Application.UseCases.Auth;

// Commands
// Commands
public record RegisterCommand(RegisterDto RegisterDto) : IRequest<AuthResponseDto>;

public record AssignRolePermissionCommand(string RoleId, int PermissionId) : IRequest<Unit>;
public record RevokeRolePermissionCommand(string RoleId, int PermissionId) : IRequest<Unit>;

public record SetUserLocationAccessCommand(string UserId, List<int> LocationIds) : IRequest<Unit>;

public record RefreshTokenCommand(string AccessToken, string RefreshToken, string? DeviceInfo, string? IPAddress) : IRequest<AuthResponseDto>;

// Queries  
public record LoginQuery(LoginDto LoginDto) : IRequest<AuthResponseDto>;

public record GetRolePermissionsQuery(string RoleId) : IRequest<List<RolePermissionDto>>;
public record GetUserLocationAccessQuery(string UserId) : IRequest<List<UserLocationAccessDto>>;

public record GetCurrentUserQuery(string UserId) : IRequest<UserInfoDto>;
