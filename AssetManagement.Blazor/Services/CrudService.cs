using System.Net.Http.Json;

namespace AssetManagement.Blazor.Services;

public class CrudService<T>
{
    private readonly HttpClient _httpClient;
    private readonly string _resource;

    public CrudService(HttpClient httpClient, string resource)
    {
        _httpClient = httpClient;
        _resource = resource;
    }

    public async Task<List<T>> GetAllAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<T>>($"api/{_resource}") ?? [];
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<T>($"api/{_resource}/{id}");
    }

    public async Task<int?> CreateAsync(T dto)
    {
        var response = await _httpClient.PostAsJsonAsync($"api/{_resource}", dto);
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        return await response.Content.ReadFromJsonAsync<int>();
    }

    public async Task<bool> UpdateAsync(T dto)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/{_resource}", dto);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"api/{_resource}/{id}");
        return response.IsSuccessStatusCode;
    }
}
