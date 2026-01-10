using System.Collections.Generic;
using System.Threading.Tasks;
using AssetManagement.Application.DTOs;
using AssetManagement.Domain.Entities;

namespace AssetManagement.Application.Interfaces
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
        Task<IEnumerable<Employee>> GetAllEmployeesAsync(IEnumerable<int>? allowedLocationIds = null);
        Task<IEnumerable<Assignment>> GetAssignmentsByEmployeeAsync(int employeeId, IEnumerable<int>? allowedLocationIds = null);
        Task<IEnumerable<Assignment>> GetAllAssignmentHistoryForEmployeeAsync(int employeeId, IEnumerable<int>? allowedLocationIds = null);
        Task<IEnumerable<Assignment>> GetAssignmentsByDateRangeAsync(DateTime startDate, DateTime endDate, IEnumerable<int>? allowedLocationIds = null);
        Task<IEnumerable<EmployeeAssetValueSummaryDto>> GetEmployeeAssetValueSummaryAsync(IEnumerable<int>? allowedLocationIds = null);
        Task<IEnumerable<DirectorateAssignmentSummaryDto>> GetAssignmentSummaryByDirectorateAsync(IEnumerable<int>? allowedLocationIds = null);
        Task<IEnumerable<Assignment>> GetOverdueAssignmentsAsync(IEnumerable<int>? allowedLocationIds = null);
    }
}