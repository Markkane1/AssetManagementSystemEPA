using System.Collections.Generic;
using System.Threading.Tasks;
using AssetManagement.Application.DTOs;
using AssetManagement.Domain.Entities;

namespace AssetManagement.Application.Interfaces
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
        Task<IEnumerable<Assignment>> GetAssignmentsByEmployeeAsync(int employeeId);
        Task<IEnumerable<Assignment>> GetAllAssignmentHistoryForEmployeeAsync(int employeeId);
        Task<IEnumerable<Assignment>> GetAssignmentsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<EmployeeAssetValueSummaryDto>> GetEmployeeAssetValueSummaryAsync();
        Task<IEnumerable<DirectorateAssignmentSummaryDto>> GetAssignmentSummaryByDirectorateAsync();
        Task<IEnumerable<Assignment>> GetOverdueAssignmentsAsync();
    }
}