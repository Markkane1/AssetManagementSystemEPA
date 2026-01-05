using AssetManagement.Application.DTOs.Auth;
using MediatR;

namespace AssetManagement.Application.UseCases.Auth;

public record GetAllPermissionsQuery : IRequest<List<PermissionDto>>;

public record GetPermissionsByCategoryQuery(string Category) : IRequest<List<PermissionDto>>;

public record GetPermissionCategoriesQuery : IRequest<List<string>>;
