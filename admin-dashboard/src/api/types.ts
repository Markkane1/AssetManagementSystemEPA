/* eslint-disable */
// Auto-generated from openapi.json. Do not edit manually.
export namespace components {
  export namespace schemas {
    export interface AssetAssignmentReportDto {
      assetItemId?: number;
      assetName?: string;
      locationName?: string;
      serialNumber?: string;
      tag?: string;
      assignmentStatus?: AssignmentStatus;
      itemStatus?: ItemStatus;
      itemSource?: ItemSource;
      assignedTo?: string;
      assignmentDate?: string;
      returnDate?: string;
    }
    export interface AssetDto {
      id?: number;
      name?: string;
      assetCode?: string;
      source?: AssetSource;
      price?: number;
      quantity?: number;
      untrackedQuantity?: number;
      trackedQuantity?: number;
      categoryId?: number;
      categoryName?: string;
      categoryDescription?: string;
      vendorId?: number;
      vendorName?: string;
      vendorAddress?: string;
      vendorContactNumber?: string;
      purchaseDate?: string;
      deliveryDate?: string;
      purchaseOrderId?: number;
      purchaseOrderNumber?: string;
      manufacturer?: string;
      model?: string;
      specification?: string;
      warrantyEndDate?: string;
      createdAt?: string;
      createdBy?: string;
      updatedAt?: string;
      updatedBy?: string;
    }
    export interface AssetItemDto {
      id?: number;
      assetId: number;
      locationId: number;
      serialNumber: string;
      tag: string;
      assignedToUserId?: number;
      assignedOn?: string;
      status: ItemStatus;
      source: ItemSource;
    }
    export type AssetSource = number;
    export interface AssetStatusReportDto {
      assignmentStatus?: AssignmentStatus;
      count?: number;
      assetItems?: AssetItemDto[];
      key?: AssignmentStatus;
      assetItemDtos?: AssetItemDto[];
    }
    export interface AssetSummaryDto {
      locationId?: number;
      locationName?: string;
      totalAssets?: number;
      totalValue?: number;
    }
    export interface AssignRolePermissionRequest {
      permissionId?: number;
    }
    export interface AssignmentDto {
      id?: number;
      assetItemId?: number;
      assetItemSerialNumber?: string;
      employeeId?: number;
      employeeName?: string;
      assignedDate?: string;
      returnedDate?: string;
      returnNotes?: string;
      assignedByEmployeeId?: number;
      returnedByEmployeeId?: number;
      assignmentStatus?: AssignmentStatus;
    }
    export type AssignmentStatus = number;
    export interface AuditTrailDto {
      assetId?: number;
      actionType?: string;
      actionDate?: string;
      details?: string;
      assignmentDate?: string;
    }
    export interface AuthResponseDto {
      token?: string;
      refreshToken?: string;
      expiresAt?: string;
      user?: UserInfoDto;
    }
    export interface CategoryDto {
      id?: number;
      name?: string;
      description?: string;
    }
    export interface CategorySummaryDto {
      categoryId?: number;
      categoryName?: string;
      totalAssets?: number;
      totalValue?: number;
    }
    export interface DepreciationReportDto {
      assetId?: number;
      assetName?: string;
      originalPrice?: number;
      currentValue?: number;
      acquisitionDate?: string;
    }
    export interface DirectorateAssignmentSummaryDto {
      directorateId?: number;
      directorateName?: string;
      totalAssignments?: number;
      totalAssetItems?: number;
    }
    export interface DirectorateDto {
      id?: number;
      name?: string;
    }
    export interface EmployeeAssetValueSummaryDto {
      employeeId?: number;
      employeeName?: string;
      totalAssetValue?: number;
    }
    export interface EmployeeDto {
      id?: number;
      name?: string;
      email?: string;
      directorateId?: number;
      directorateName?: string;
    }
    export type ItemSource = number;
    export type ItemStatus = number;
    export interface LocationDto {
      id?: number;
      name?: string;
      isHeadOffice?: boolean;
      district?: string;
    }
    export interface LocationTransferSummaryDto {
      fromLocationId?: number;
      toLocationId?: number;
      transferCount?: number;
      totalValue?: number;
    }
    export interface LoginDto {
      username?: string;
      password?: string;
    }
    export interface MaintenanceCostSummaryDto {
      assetId?: number;
      categoryId?: number;
      locationId?: number;
      totalCost?: number;
      value?: unknown;
      v?: number;
    }
    export interface MaintenanceDueReportDto {
      assetItemId?: number;
      assetName?: string;
      dueDate?: string;
    }
    export interface MaintenanceRecordDto {
      id?: number;
      assetItemId?: number;
      assetItemSerialNumber?: string;
      date?: string;
      description?: string;
      cost?: number;
      status?: string;
    }
    export interface NonRepairableAssetReportDto {
      assetItemId?: number;
      assetName?: string;
      locationName?: string;
      serialNumber?: string;
      tag?: string;
      assignmentStatus?: AssignmentStatus;
      itemStatus?: ItemStatus;
      itemSource?: ItemSource;
    }
    export interface OverstockReportDto {
      assetId?: number;
      assetName?: string;
      currentQuantity?: number;
      threshold?: number;
      id?: number;
      name?: string;
      quantity?: number;
    }
    export interface PermissionDto {
      id?: number;
      name?: string;
      description?: string;
      category?: string;
      isActive?: boolean;
    }
    export interface ProjectDto {
      id?: number;
      name?: string;
      projectDescription?: string;
      projectRunBy?: string;
    }
    export interface PurchaseOrderDto {
      id?: number;
      orderNumber?: string;
      orderDate?: string;
      vendorId?: number;
      vendorName?: string;
    }
    export interface ReassignAssetItemRequest {
      assignmentId?: number;
      newEmployeeId?: number;
      assignmentDate?: string;
    }
    export interface RefreshTokenRequest {
      accessToken?: string;
      refreshToken?: string;
    }
    export interface RegisterDto {
      email?: string;
      username?: string;
      password?: string;
      firstName?: string;
      lastName?: string;
    }
    export interface ReturnAssetItemRequest {
      assetItemId?: number;
      returnDate?: string;
    }
    export interface RolePermissionDto {
      id?: number;
      roleId?: string;
      permissionId?: number;
      permissionName?: string;
    }
    export interface TransferAssetsRequest {
      assetItemIds?: number[];
      targetLocationId?: number;
    }
    export interface TransferHistoryDto {
      id?: number;
      assetItemId?: number;
      fromLocationId?: number;
      toLocationId?: number;
      transferDate?: string;
    }
    export interface UserInfoDto {
      id?: string;
      username?: string;
      email?: string;
      firstName?: string;
      lastName?: string;
      permissions?: string[];
    }
    export interface UserLocationAccessDto {
      userId?: string;
      locationId?: number;
      locationName?: string;
    }
    export interface UtilizationReportDto {
      assetId?: number;
      assetName?: string;
      utilizationRate?: number;
    }
    export interface VendorDto {
      id?: number;
      name?: string;
      address?: string;
      contactNumber?: string;
    }
  }
}