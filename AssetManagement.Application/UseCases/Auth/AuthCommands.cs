using AssetManagement.Application.DTOs.Auth;
using MediatR;

namespace AssetManagement.Application.UseCases.Auth;

// Commands
public record RegisterCommand(RegisterDto RegisterDto) : IRequest<AuthResponseDto>;

public record AssignPermissionCommand(string UserId, int PermissionId, string GrantedBy) : IRequest<Unit>;

public record RevokePermissionCommand(string UserId, int PermissionId) : IRequest<Unit>;

// Queries  
public record LoginQuery(LoginDto LoginDto) : IRequest<AuthResponseDto>;

public record GetUserPermissionsQuery(string UserId) : IRequest<List<UserPermissionDto>>;

public record GetCurrentUserQuery(string UserId) : IRequest<UserInfoDto>;
