using System.Net.Http.Json;
using AssetManagement.Application.DTOs;

namespace AssetManagement.Blazor.Services;

public class AssignmentService
{
    private readonly HttpClient _httpClient;
    private readonly CrudService<AssignmentDto> _crudService;

    public AssignmentService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _crudService = new CrudService<AssignmentDto>(httpClient, "assignments");
    }

    public Task<List<AssignmentDto>> GetAllAsync() => _crudService.GetAllAsync();
    public Task<AssignmentDto?> GetByIdAsync(int id) => _crudService.GetByIdAsync(id);
    public Task<int?> CreateAsync(AssignmentDto dto) => _crudService.CreateAsync(dto);
    public Task<bool> UpdateAsync(AssignmentDto dto) => _crudService.UpdateAsync(dto);
    public Task<bool> DeleteAsync(int id) => _crudService.DeleteAsync(id);

    public async Task<bool> ReturnAsync(int assetItemId, DateTime returnDate)
    {
        var response = await _httpClient.PostAsJsonAsync("api/assignments/return",
            new ReturnAssetItemRequest(assetItemId, returnDate));
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> ReassignAsync(int assignmentId, int newEmployeeId, DateTime assignmentDate)
    {
        var response = await _httpClient.PostAsJsonAsync("api/assignments/reassign",
            new ReassignAssetItemRequest(assignmentId, newEmployeeId, assignmentDate));
        return response.IsSuccessStatusCode;
    }

    public async Task<List<AssignmentDto>> GetByEmployeeAsync(int employeeId)
    {
        return await _httpClient.GetFromJsonAsync<List<AssignmentDto>>($"api/assignments/by-employee/{employeeId}") ?? [];
    }

    public async Task<List<AssignmentDto>> GetByAssetItemAsync(int assetItemId)
    {
        return await _httpClient.GetFromJsonAsync<List<AssignmentDto>>($"api/assignments/by-asset-item/{assetItemId}") ?? [];
    }

    public async Task<List<AssignmentDto>> GetHistoryForEmployeeAsync(int employeeId)
    {
        return await _httpClient.GetFromJsonAsync<List<AssignmentDto>>($"api/assignments/history/employee/{employeeId}") ?? [];
    }

    public async Task<List<AssignmentDto>> GetHistoryForAssetAsync(int assetItemId)
    {
        return await _httpClient.GetFromJsonAsync<List<AssignmentDto>>($"api/assignments/history/asset/{assetItemId}") ?? [];
    }

    public async Task<List<AssignmentDto>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        var query = $"api/assignments/by-date-range?startDate={startDate:O}&endDate={endDate:O}";
        return await _httpClient.GetFromJsonAsync<List<AssignmentDto>>(query) ?? [];
    }

    public async Task<List<AssignmentDto>> GetOverdueAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<AssignmentDto>>("api/assignments/overdue") ?? [];
    }

    private record ReturnAssetItemRequest(int AssetItemId, DateTime ReturnDate);
    private record ReassignAssetItemRequest(int AssignmentId, int NewEmployeeId, DateTime AssignmentDate);
}
