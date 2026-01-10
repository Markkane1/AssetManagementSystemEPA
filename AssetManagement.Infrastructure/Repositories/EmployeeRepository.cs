using AssetManagement.Application.DTOs;
using AssetManagement.Application.Interfaces;
using AssetManagement.Domain.Entities;
using AssetManagement.Domain.Enums;
using AssetManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssetManagement.Infrastructure.Repositories
{
    public class EmployeeRepository : Repository<Employee>, IEmployeeRepository
    {
        private readonly AppDbContext _context;

        public EmployeeRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Employee>> GetAllEmployeesAsync(IEnumerable<int>? allowedLocationIds = null)
        {
            var query = _context.Employees.AsQueryable();
            if (allowedLocationIds != null && allowedLocationIds.Any())
                query = query.Where(e => allowedLocationIds.Contains(e.LocationId));
            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Assignment>> GetAssignmentsByEmployeeAsync(int employeeId, IEnumerable<int>? allowedLocationIds = null)
        {
            var query = _context.Assignments.AsQueryable();
            if (allowedLocationIds != null && allowedLocationIds.Any())
                query = query.Where(a => allowedLocationIds.Contains(a.AssetItem.LocationId));

            return await query
                .Where(a => a.EmployeeId == employeeId && a.ReturnDate == null)
                .Include(a => a.AssetItem)
                .ThenInclude(ai => ai.Asset)
                .ToListAsync();
        }

        public async Task<IEnumerable<Assignment>> GetAllAssignmentHistoryForEmployeeAsync(int employeeId, IEnumerable<int>? allowedLocationIds = null)
        {
            var query = _context.Assignments.AsQueryable();
            if (allowedLocationIds != null && allowedLocationIds.Any())
                query = query.Where(a => allowedLocationIds.Contains(a.AssetItem.LocationId));

            return await query
                .Where(a => a.EmployeeId == employeeId)
                .Include(a => a.AssetItem)
                .ThenInclude(ai => ai.Asset)
                .ToListAsync();
        }

        public async Task<IEnumerable<Assignment>> GetAssignmentsByDateRangeAsync(DateTime startDate, DateTime endDate, IEnumerable<int>? allowedLocationIds = null)
        {
            var query = _context.Assignments.AsQueryable();
            if (allowedLocationIds != null && allowedLocationIds.Any())
                query = query.Where(a => allowedLocationIds.Contains(a.AssetItem.LocationId));

            return await query
                .Where(a => a.AssignmentDate >= startDate && a.AssignmentDate <= endDate)
                .Include(a => a.AssetItem)
                .ThenInclude(ai => ai.Asset)
                .ToListAsync();
        }

        public async Task<IEnumerable<Assignment>> GetOverdueAssignmentsAsync(IEnumerable<int>? allowedLocationIds = null)
        {
            var query = _context.Assignments.AsQueryable();
            if (allowedLocationIds != null && allowedLocationIds.Any())
                query = query.Where(a => allowedLocationIds.Contains(a.AssetItem.LocationId));

            return await query
                .Where(a => a.ReturnDate.HasValue && a.ReturnDate.Value < DateTime.UtcNow && a.AssignmentStatus == AssignmentStatus.Assigned)
                .Include(a => a.AssetItem)
                .ThenInclude(ai => ai.Asset)
                .ToListAsync();
        }

        public async Task<IEnumerable<EmployeeAssetValueSummaryDto>> GetEmployeeAssetValueSummaryAsync(IEnumerable<int>? allowedLocationIds = null)
        {
            var query = _context.Employees.AsQueryable();
            if (allowedLocationIds != null && allowedLocationIds.Any())
                query = query.Where(e => allowedLocationIds.Contains(e.LocationId));

            return await query
                .Select(e => new EmployeeAssetValueSummaryDto
                {
                    EmployeeId = e.Id,
                    EmployeeName = $"{e.FirstName} {e.LastName}",
                    TotalAssetValue = e.Assignments
                        .Where(a => a.ReturnDate == null)
                        .Sum(a => a.AssetItem.Asset.Price.Amount)
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<DirectorateAssignmentSummaryDto>> GetAssignmentSummaryByDirectorateAsync(IEnumerable<int>? allowedLocationIds = null)
        {
            return await _context.Directorates
                .Select(d => new DirectorateAssignmentSummaryDto
                {
                    DirectorateId = d.Id,
                    DirectorateName = d.Name,
                    TotalAssignments = d.Employees
                        .Where(e => allowedLocationIds == null || !allowedLocationIds.Any() || allowedLocationIds.Contains(e.LocationId))
                        .SelectMany(e => e.Assignments).Count(),
                    TotalAssetItems = d.Employees
                        .Where(e => allowedLocationIds == null || !allowedLocationIds.Any() || allowedLocationIds.Contains(e.LocationId))
                        .SelectMany(e => e.Assignments).Select(a => a.AssetItemId).Distinct().Count()
                })
                .ToListAsync();
        }
    }
}