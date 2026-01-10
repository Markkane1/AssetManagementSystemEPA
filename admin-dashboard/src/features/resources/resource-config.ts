import type { ResourceConfig } from "@/src/api/resources";

export const resourceConfigs: ResourceConfig[] = [
  {
    key: "assets",
    label: "Assets",
    basePath: "/api/Assets",
    createSchema: "AssetDto",
    updateSchema: "AssetDto",
    detailSchema: "AssetDto",
    permissions: {
      view: "Assets.View",
      create: "Assets.Create",
      edit: "Assets.Edit",
      delete: "Assets.Delete"
    }
  },
  {
    key: "asset-items",
    label: "Asset Items",
    basePath: "/api/AssetItems",
    createSchema: "AssetItemDto",
    updateSchema: "AssetItemDto",
    detailSchema: "AssetItemDto",
    permissions: {
      view: "AssetItems.View",
      create: "AssetItems.Create",
      edit: "AssetItems.Edit",
      delete: "AssetItems.Delete"
    }
  },
  {
    key: "assignments",
    label: "Assignments",
    basePath: "/api/Assignments",
    createSchema: "AssignmentDto",
    updateSchema: "AssignmentDto",
    detailSchema: "AssignmentDto",
    permissions: {
      view: "Assignments.View",
      create: "Assignments.Create",
      edit: "Assignments.Edit",
      delete: "Assignments.Delete"
    }
  },
  {
    key: "categories",
    label: "Categories",
    basePath: "/api/Categories",
    createSchema: "CategoryDto",
    updateSchema: "CategoryDto",
    detailSchema: "CategoryDto",
    permissions: {
      view: "Categories.View",
      create: "Categories.Create",
      edit: "Categories.Edit",
      delete: "Categories.Delete"
    }
  },
  {
    key: "directorates",
    label: "Directorates",
    basePath: "/api/Directorates",
    createSchema: "DirectorateDto",
    updateSchema: "DirectorateDto",
    detailSchema: "DirectorateDto",
    permissions: {
      view: "Directorates.View",
      create: "Directorates.Create",
      edit: "Directorates.Edit",
      delete: "Directorates.Delete"
    }
  },
  {
    key: "employees",
    label: "Employees",
    basePath: "/api/Employees",
    createSchema: "EmployeeDto",
    updateSchema: "EmployeeDto",
    detailSchema: "EmployeeDto",
    permissions: {
      view: "Employees.View",
      create: "Employees.Create",
      edit: "Employees.Edit",
      delete: "Employees.Delete"
    }
  },
  {
    key: "locations",
    label: "Locations",
    basePath: "/api/Locations",
    createSchema: "LocationDto",
    updateSchema: "LocationDto",
    detailSchema: "LocationDto",
    permissions: {
      view: "Locations.View",
      create: "Locations.Create",
      edit: "Locations.Edit",
      delete: "Locations.Delete"
    }
  },
  {
    key: "maintenance-records",
    label: "Maintenance Records",
    basePath: "/api/MaintenanceRecords",
    createSchema: "MaintenanceRecordDto",
    updateSchema: "MaintenanceRecordDto",
    detailSchema: "MaintenanceRecordDto",
    permissions: {
      view: "MaintenanceRecords.View",
      create: "MaintenanceRecords.Create",
      edit: "MaintenanceRecords.Edit",
      delete: "MaintenanceRecords.Delete"
    }
  },
  {
    key: "projects",
    label: "Projects",
    basePath: "/api/Projects",
    createSchema: "ProjectDto",
    updateSchema: "ProjectDto",
    detailSchema: "ProjectDto",
    permissions: {
      view: "Projects.View",
      create: "Projects.Create",
      edit: "Projects.Edit",
      delete: "Projects.Delete"
    }
  },
  {
    key: "purchase-orders",
    label: "Purchase Orders",
    basePath: "/api/PurchaseOrders",
    createSchema: "PurchaseOrderDto",
    updateSchema: "PurchaseOrderDto",
    detailSchema: "PurchaseOrderDto",
    permissions: {
      view: "PurchaseOrders.View",
      create: "PurchaseOrders.Create",
      edit: "PurchaseOrders.Edit",
      delete: "PurchaseOrders.Delete"
    }
  },
  {
    key: "vendors",
    label: "Vendors",
    basePath: "/api/Vendors",
    createSchema: "VendorDto",
    updateSchema: "VendorDto",
    detailSchema: "VendorDto",
    permissions: {
      view: "Vendors.View",
      create: "Vendors.Create",
      edit: "Vendors.Edit",
      delete: "Vendors.Delete"
    }
  }
];

export function getResourceConfig(key: string) {
  return resourceConfigs.find((resource) => resource.key === key);
}
