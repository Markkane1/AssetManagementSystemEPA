using System.Net.Http.Json;
using AssetManagement.Application.DTOs;

namespace AssetManagement.Blazor.Services;

public class AssetItemService
{
    private readonly HttpClient _httpClient;
    private readonly CrudService<AssetItemDto> _crudService;

    public AssetItemService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _crudService = new CrudService<AssetItemDto>(httpClient, "assetitems");
    }

    public Task<List<AssetItemDto>> GetAllAsync() => _crudService.GetAllAsync();
    public Task<AssetItemDto?> GetByIdAsync(int id) => _crudService.GetByIdAsync(id);
    public Task<int?> CreateAsync(AssetItemDto dto) => _crudService.CreateAsync(dto);
    public Task<bool> UpdateAsync(AssetItemDto dto) => _crudService.UpdateAsync(dto);
    public Task<bool> DeleteAsync(int id) => _crudService.DeleteAsync(id);

    public async Task<bool> TransferAsync(int[] assetItemIds, int targetLocationId)
    {
        var response = await _httpClient.PostAsJsonAsync("api/assetitems/transfer",
            new TransferAssetsRequest(assetItemIds, targetLocationId));
        return response.IsSuccessStatusCode;
    }

    public async Task<List<AssetItemDto>> GetUnassignedAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<AssetItemDto>>("api/assetitems/unassigned") ?? [];
    }

    private record TransferAssetsRequest(int[] AssetItemIds, int TargetLocationId);
}
