using AssetManagement.Application.DTOs;

namespace AssetManagement.Blazor.Services;

public class CategoryService : CrudService<CategoryDto>
{
    public CategoryService(HttpClient httpClient) : base(httpClient, "categories") { }
}

public class DirectorateService : CrudService<DirectorateDto>
{
    public DirectorateService(HttpClient httpClient) : base(httpClient, "directorates") { }
}

public class EmployeeService : CrudService<EmployeeDto>
{
    public EmployeeService(HttpClient httpClient) : base(httpClient, "employees") { }
}

public class LocationService : CrudService<LocationDto>
{
    public LocationService(HttpClient httpClient) : base(httpClient, "locations") { }
}

public class MaintenanceRecordService : CrudService<MaintenanceRecordDto>
{
    public MaintenanceRecordService(HttpClient httpClient) : base(httpClient, "maintenancerecords") { }
}

public class ProjectService : CrudService<ProjectDto>
{
    public ProjectService(HttpClient httpClient) : base(httpClient, "projects") { }
}

public class PurchaseOrderService : CrudService<PurchaseOrderDto>
{
    public PurchaseOrderService(HttpClient httpClient) : base(httpClient, "purchaseorders") { }
}

public class VendorService : CrudService<VendorDto>
{
    public VendorService(HttpClient httpClient) : base(httpClient, "vendors") { }
}
