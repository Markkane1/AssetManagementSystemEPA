using AssetManagement.Blazor.Services;
using AssetManagement.Blazor.Services.Auth;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using System.Net.Http;


namespace AssetManagement.Blazor
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            builder.Services.AddMudServices();
            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddAuthorizationCore(options =>
            {
                options.FallbackPolicy = new Microsoft.AspNetCore.Authorization.AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
            });
            builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();
            builder.Services.AddScoped<AuthService>();
            builder.Services.AddScoped<ApiAuthMessageHandler>();
            builder.Services.AddScoped<PermissionService>();

            var apiBaseUrl = builder.Configuration["ApiBaseUrl"] ?? builder.HostEnvironment.BaseAddress;
            builder.Services.AddHttpClient("ApiClient", client => client.BaseAddress = new Uri(apiBaseUrl))
                .AddHttpMessageHandler<ApiAuthMessageHandler>();
            builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("ApiClient"));

            builder.Services.AddScoped<AssetService>();
            builder.Services.AddScoped<AssetItemService>();
            builder.Services.AddScoped<AssignmentService>();
            builder.Services.AddScoped<CategoryService>();
            builder.Services.AddScoped<DirectorateService>();
            builder.Services.AddScoped<EmployeeService>();
            builder.Services.AddScoped<LocationService>();
            builder.Services.AddScoped<MaintenanceRecordService>();
            builder.Services.AddScoped<ProjectService>();
            builder.Services.AddScoped<PurchaseOrderService>();
            builder.Services.AddScoped<VendorService>();
            builder.Services.AddScoped<ReportService>();
            builder.Services.AddScoped<RoleService>();
            builder.Services.AddScoped<PermissionCatalogService>();
            builder.Services.AddScoped<UserAccessService>();

            await builder.Build().RunAsync();
        }
    }
}
