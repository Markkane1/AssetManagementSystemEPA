using System.Net.Http.Json;
using AssetManagement.Application.DTOs;

namespace AssetManagement.Blazor.Services;

public class AssetService
{
    private readonly HttpClient _httpClient;
    private readonly CrudService<AssetDto> _crudService;

    public AssetService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _crudService = new CrudService<AssetDto>(httpClient, "assets");
    }

    public Task<List<AssetDto>> GetAllAsync() => _crudService.GetAllAsync();
    public Task<AssetDto?> GetByIdAsync(int id) => _crudService.GetByIdAsync(id);
    public Task<int?> CreateAsync(AssetDto dto) => _crudService.CreateAsync(dto);
    public Task<bool> UpdateAsync(AssetDto dto) => _crudService.UpdateAsync(dto);
    public Task<bool> DeleteAsync(int id) => _crudService.DeleteAsync(id);

    public async Task<List<AssetDto>> GetByLocationAsync(int locationId)
    {
        return await _httpClient.GetFromJsonAsync<List<AssetDto>>($"api/assets/by-location/{locationId}") ?? [];
    }

    public async Task<List<AssetDto>> GetByCategoryAsync(int categoryId)
    {
        return await _httpClient.GetFromJsonAsync<List<AssetDto>>($"api/assets/by-category/{categoryId}") ?? [];
    }

    public async Task<List<AssetDto>> GetBySourceAsync(int sourceId, string sourceType)
    {
        return await _httpClient.GetFromJsonAsync<List<AssetDto>>($"api/assets/by-source/{sourceId}/{sourceType}") ?? [];
    }

    public async Task<List<AssetDto>> GetByVendorAsync(int vendorId)
    {
        return await _httpClient.GetFromJsonAsync<List<AssetDto>>($"api/assets/by-vendor/{vendorId}") ?? [];
    }

    public async Task<List<AssetDto>> GetByAcquisitionDateAsync(DateTime startDate, DateTime endDate)
    {
        var query = $"api/assets/by-acquisition-date?startDate={startDate:O}&endDate={endDate:O}";
        return await _httpClient.GetFromJsonAsync<List<AssetDto>>(query) ?? [];
    }

    public async Task<List<AssetDto>> GetLowStockAsync(int threshold)
    {
        return await _httpClient.GetFromJsonAsync<List<AssetDto>>($"api/assets/low-stock/{threshold}") ?? [];
    }
}
