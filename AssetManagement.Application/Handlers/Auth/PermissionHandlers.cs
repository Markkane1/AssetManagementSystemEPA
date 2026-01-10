using AssetManagement.Application.DTOs.Auth;
using AssetManagement.Application.UseCases.Auth;
using AssetManagement.Domain.Entities;
using AssetManagement.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AssetManagement.Application.Handlers.Auth;

public class AssignRolePermissionCommandHandler : IRequestHandler<AssignRolePermissionCommand, Unit>
{
    private readonly IAppDbContext _context;

    public AssignRolePermissionCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(AssignRolePermissionCommand request, CancellationToken cancellationToken)
    {
        var permission = await _context.Permissions.FindAsync(new object[] { request.PermissionId }, cancellationToken);
        if (permission == null)
            throw new KeyNotFoundException($"Permission with ID {request.PermissionId} not found");

        var existing = await _context.RolePermissions
            .FirstOrDefaultAsync(rp => rp.RoleId == request.RoleId && rp.PermissionId == request.PermissionId, cancellationToken);

        if (existing != null)
            return Unit.Value;

        var rolePermission = new RolePermission(request.RoleId, request.PermissionId);
        await _context.RolePermissions.AddAsync(rolePermission, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}

public class RevokeRolePermissionCommandHandler : IRequestHandler<RevokeRolePermissionCommand, Unit>
{
    private readonly IAppDbContext _context;

    public RevokeRolePermissionCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(RevokeRolePermissionCommand request, CancellationToken cancellationToken)
    {
        var rolePermission = await _context.RolePermissions
            .FirstOrDefaultAsync(rp => rp.RoleId == request.RoleId && rp.PermissionId == request.PermissionId, cancellationToken);

        if (rolePermission == null)
            throw new KeyNotFoundException("Role permission not found");

        _context.RolePermissions.Remove(rolePermission);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}

public class SetUserLocationAccessCommandHandler : IRequestHandler<SetUserLocationAccessCommand, Unit>
{
    private readonly IAppDbContext _context;

    public SetUserLocationAccessCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(SetUserLocationAccessCommand request, CancellationToken cancellationToken)
    {
        // Remove existing
        var currentAccess = await _context.UserLocationAccess
            .Where(ula => ula.UserId == request.UserId)
            .ToListAsync(cancellationToken);

        _context.UserLocationAccess.RemoveRange(currentAccess);

        // Add new
        if (request.LocationIds != null && request.LocationIds.Any())
        {
            var accessList = request.LocationIds.Select(locId => new UserLocationAccess(request.UserId, locId));
            await _context.UserLocationAccess.AddRangeAsync(accessList, cancellationToken);
        }

        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}

public class GetRolePermissionsQueryHandler : IRequestHandler<GetRolePermissionsQuery, List<RolePermissionDto>>
{
    private readonly IAppDbContext _context;

    public GetRolePermissionsQueryHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<List<RolePermissionDto>> Handle(GetRolePermissionsQuery request, CancellationToken cancellationToken)
    {
        return await _context.RolePermissions
            .Where(rp => rp.RoleId == request.RoleId)
            .Include(rp => rp.Permission)
            .Select(rp => new RolePermissionDto
            {
                Id = rp.Id,
                RoleId = rp.RoleId,
                PermissionId = rp.PermissionId,
                PermissionName = rp.Permission.Name
            })
            .ToListAsync(cancellationToken);
    }
}

public class GetUserLocationAccessQueryHandler : IRequestHandler<GetUserLocationAccessQuery, List<UserLocationAccessDto>>
{
    private readonly IAppDbContext _context;

    public GetUserLocationAccessQueryHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<List<UserLocationAccessDto>> Handle(GetUserLocationAccessQuery request, CancellationToken cancellationToken)
    {
        return await _context.UserLocationAccess
            .Where(ula => ula.UserId == request.UserId)
            .Include(ula => ula.Location)
            .Select(ula => new UserLocationAccessDto
            {
                UserId = ula.UserId,
                LocationId = ula.LocationId,
                LocationName = ula.Location.Name
            })
            .ToListAsync(cancellationToken);
    }
}

public class GetAllPermissionsQueryHandler : IRequestHandler<GetAllPermissionsQuery, List<PermissionDto>>
{
    private readonly IAppDbContext _context;

    public GetAllPermissionsQueryHandler(IAppDbContext context)
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
    private readonly IAppDbContext _context;

    public GetPermissionsByCategoryQueryHandler(IAppDbContext context)
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
    private readonly IAppDbContext _context;

    public GetPermissionCategoriesQueryHandler(IAppDbContext context)
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
