using AssetManagement.Application.DTOs.Auth;
using AssetManagement.Application.UseCases.Auth;
using AssetManagement.Domain.Entities;
using AssetManagement.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AssetManagement.Infrastructure.Handlers.Auth;

public class AssignPermissionCommandHandler : IRequestHandler<AssignPermissionCommand, Unit>
{
    private readonly AppDbContext _context;

    public AssignPermissionCommandHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(AssignPermissionCommand request, CancellationToken cancellationToken)
    {
        // Check if permission exists
        var permission = await _context.Permissions.FindAsync(new object[] { request.PermissionId }, cancellationToken);
        if (permission == null)
            throw new KeyNotFoundException($"Permission with ID {request.PermissionId} not found");

        // Check if already assigned
        var existing = await _context.UserPermissions
            .FirstOrDefaultAsync(up => up.UserId == request.UserId && up.PermissionId == request.PermissionId, cancellationToken);

        if (existing != null)
            return Unit.Value; // Already assigned

        // Create new user permission
        var userPermission = new UserPermission(request.UserId, request.PermissionId, request.GrantedBy);
        await _context.UserPermissions.AddAsync(userPermission, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}

public class RevokePermissionCommandHandler : IRequestHandler<RevokePermissionCommand, Unit>
{
    private readonly AppDbContext _context;

    public RevokePermissionCommandHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(RevokePermissionCommand request, CancellationToken cancellationToken)
    {
        var userPermission = await _context.UserPermissions
            .FirstOrDefaultAsync(up => up.UserId == request.UserId && up.PermissionId == request.PermissionId, cancellationToken);

        if (userPermission == null)
            throw new KeyNotFoundException("User permission not found");

        _context.UserPermissions.Remove(userPermission);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}

public class GetUserPermissionsQueryHandler : IRequestHandler<GetUserPermissionsQuery, List<UserPermissionDto>>
{
    private readonly AppDbContext _context;

    public GetUserPermissionsQueryHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<UserPermissionDto>> Handle(GetUserPermissionsQuery request, CancellationToken cancellationToken)
    {
        return await _context.UserPermissions
            .Where(up => up.UserId == request.UserId)
            .Include(up => up.Permission)
            .Select(up => new UserPermissionDto
            {
                Id = up.Id,
                UserId = up.UserId,
                PermissionId = up.PermissionId,
                PermissionName = up.Permission.Name,
                GrantedAt = up.GrantedAt,
                GrantedBy = up.GrantedBy
            })
            .ToListAsync(cancellationToken);
    }
}

public class GetAllPermissionsQueryHandler : IRequestHandler<GetAllPermissionsQuery, List<PermissionDto>>
{
    private readonly AppDbContext _context;

    public GetAllPermissionsQueryHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<PermissionDto>> Handle(GetAllPermissionsQuery request, CancellationToken cancellationToken)
    {
        return await _context.Permissions
            .Select(p => new PermissionDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Category = p.Category,
                IsActive = p.IsActive
            })
            .ToListAsync(cancellationToken);
    }
}

public class GetPermissionsByCategoryQueryHandler : IRequestHandler<GetPermissionsByCategoryQuery, List<PermissionDto>>
{
    private readonly AppDbContext _context;

    public GetPermissionsByCategoryQueryHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<PermissionDto>> Handle(GetPermissionsByCategoryQuery request, CancellationToken cancellationToken)
    {
        return await _context.Permissions
            .Where(p => p.Category == request.Category)
            .Select(p => new PermissionDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Category = p.Category,
                IsActive = p.IsActive
            })
            .ToListAsync(cancellationToken);
    }
}

public class GetPermissionCategoriesQueryHandler : IRequestHandler<GetPermissionCategoriesQuery, List<string>>
{
    private readonly AppDbContext _context;

    public GetPermissionCategoriesQueryHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<string>> Handle(GetPermissionCategoriesQuery request, CancellationToken cancellationToken)
    {
        return await _context.Permissions
            .Select(p => p.Category)
            .Distinct()
            .OrderBy(c => c)
            .ToListAsync(cancellationToken);
    }
}
