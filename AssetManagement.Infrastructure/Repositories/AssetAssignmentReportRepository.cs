using AssetManagement.Application.DTOs;
using AssetManagement.Application.Interfaces;
using AssetManagement.Domain.Enums;
using AssetManagement.Infrastructure;
using AssetManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AssetManagement.Infrastructure;

public class AssetAssignmentReportRepository : IAssetAssignmentReportRepository
{
    private readonly AppDbContext _context;

    public AssetAssignmentReportRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<AssetAssignmentReportDto>> GetAssetAssignmentReportAsync(CancellationToken cancellationToken = default)
    {
        return await _context.AssetItems
            .Join(_context.Assets,
                ai => ai.AssetId,
                a => a.Id,
                (ai, a) => new { AssetItem = ai, AssetName = a.Name })
            .Join(_context.Locations,
                x => x.AssetItem.LocationId,
                l => l.Id,
                (x, l) => new { x.AssetItem, x.AssetName, LocationName = l.Name })
            .GroupJoin(_context.Assignments,
                x => x.AssetItem.Id,
                ass => ass.AssetItemId,
                (x, ass) => new { x.AssetItem, x.AssetName, x.LocationName, Assignments = ass })
            .SelectMany(x => x.Assignments.DefaultIfEmpty(),
                (x, ass) => new
                {
                    x.AssetItem,
                    x.AssetName,
                    x.LocationName,
                    Assignment = ass,
                    EmployeeId = ass != null ? ass.EmployeeId : (int?)null
                })
            .GroupJoin(_context.Employees,
                x => x.EmployeeId,
                e => e.Id,
                (x, e) => new { x.AssetItem, x.AssetName, x.LocationName, x.Assignment, Employees = e })
            .SelectMany(x => x.Employees.DefaultIfEmpty(),
                (x, e) => new AssetAssignmentReportDto
                {
                    AssetItemId = x.AssetItem.Id,
                    AssetName = x.AssetName,
                    LocationName = x.LocationName,
                    SerialNumber = x.AssetItem.SerialNumber,
                    Tag = x.AssetItem.Tag,
                    AssignmentStatus = x.Assignment != null ? x.Assignment.AssignmentStatus : AssignmentStatus.Available,
                    ItemStatus = x.AssetItem.Status,
                    ItemSource = x.AssetItem.Source,
                    AssignedTo = e != null ? e.FirstName : null,
                    AssignmentDate = x.Assignment != null ? x.Assignment.AssignmentDate : null,
                    ReturnDate = x.Assignment != null ? x.Assignment.ReturnDate : null
                })
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<NonRepairableAssetReportDto>> GetNonRepairableAssetsReportAsync(int? locationId = null, CancellationToken cancellationToken = default)
    {
        var query = _context.AssetItems
            .Where(ai => ai.Status == ItemStatus.NotRepairable)
            .Join(_context.Assets,
                ai => ai.AssetId,
                a => a.Id,
                (ai, a) => new { AssetItem = ai, AssetName = a.Name })
            .Join(_context.Locations,
                x => x.AssetItem.LocationId,
                l => l.Id,
                (x, l) => new { x.AssetItem, x.AssetName, LocationName = l.Name });

        if (locationId.HasValue)
        {
            query = query.Where(x => x.AssetItem.LocationId == locationId.Value);
        }

        var result = await query
            .GroupJoin(_context.Assignments,
                x => x.AssetItem.Id,
                ass => ass.AssetItemId,
                (x, ass) => new { x.AssetItem, x.AssetName, x.LocationName, Assignments = ass })
            .SelectMany(x => x.Assignments.DefaultIfEmpty(),
                (x, ass) => new { x.AssetItem, x.AssetName, x.LocationName, Assignment = ass })
            .Where(x => x.Assignment == null || x.Assignment.ReturnDate == null) // Get active or no assignment
            .Select(x => new NonRepairableAssetReportDto
            {
                AssetItemId = x.AssetItem.Id,
                AssetName = x.AssetName,
                LocationName = x.LocationName,
                SerialNumber = x.AssetItem.SerialNumber,
                Tag = x.AssetItem.Tag,
                AssignmentStatus = x.Assignment != null ? x.Assignment.AssignmentStatus : AssignmentStatus.Available,
                ItemStatus = x.AssetItem.Status,
                ItemSource = x.AssetItem.Source
            })
            .ToListAsync(cancellationToken);

        return result;
    }
}