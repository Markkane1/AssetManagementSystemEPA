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

        public async Task<IEnumerable<Assignment>> GetAssignmentsByEmployeeAsync(int employeeId)
        {
            return await _context.Assignments
                .Where(a => a.EmployeeId == employeeId && a.ReturnDate == null)
                .Include(a => a.AssetItem)
                .ThenInclude(ai => ai.Asset)
                .ToListAsync();
        }

        public async Task<IEnumerable<Assignment>> GetAllAssignmentHistoryForEmployeeAsync(int employeeId)
        {
            return await _context.Assignments
                .Where(a => a.EmployeeId == employeeId)
                .Include(a => a.AssetItem)
                .ThenInclude(ai => ai.Asset)
                .ToListAsync();
        }

        public async Task<IEnumerable<Assignment>> GetAssignmentsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Assignments
                .Where(a => a.AssignmentDate >= startDate && a.AssignmentDate <= endDate)
                .Include(a => a.AssetItem)
                .ThenInclude(ai => ai.Asset)
                .ToListAsync();
        }

        public async Task<IEnumerable<Assignment>> GetOverdueAssignmentsAsync()
        {
            return await _context.Assignments
                .Where(a => a.ReturnDate.HasValue && a.ReturnDate.Value < DateTime.UtcNow && a.AssetItem.AssignmentStatus == AssignmentStatus.Assigned)
                .Include(a => a.AssetItem)
                .ThenInclude(ai => ai.Asset)
                .ToListAsync();
        }

        public async Task<IEnumerable<EmployeeAssetValueSummaryDto>> GetEmployeeAssetValueSummaryAsync()
        {
            return await _context.Employees
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

        public async Task<IEnumerable<DirectorateAssignmentSummaryDto>> GetAssignmentSummaryByDirectorateAsync()
        {
            return await _context.Directorates
                .Select(d => new DirectorateAssignmentSummaryDto
                {
                    DirectorateId = d.Id,
                    DirectorateName = d.Name,
                    TotalAssignments = d.Employees.SelectMany(e => e.Assignments).Count(),
                    TotalAssetItems = d.Employees.SelectMany(e => e.Assignments).Select(a => a.AssetItemId).Distinct().Count()
                })
                .ToListAsync();
        }
    }
}