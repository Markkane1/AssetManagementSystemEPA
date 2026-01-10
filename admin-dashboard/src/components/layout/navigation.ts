import { ClipboardList, FileBarChart2, LayoutDashboard, Package, Settings, Users } from "lucide-react";

export const navItems = [
  { href: "/dashboard", label: "Dashboard", icon: LayoutDashboard },
  { href: "/assets", label: "Assets", icon: Package, permission: "Assets.View" },
  { href: "/asset-items", label: "Asset Items", icon: Package, permission: "AssetItems.View" },
  { href: "/assignments", label: "Assignments", icon: ClipboardList, permission: "Assignments.View" },
  { href: "/assignments/return", label: "Return Assets", icon: ClipboardList, permission: "Assignments.Edit" },
  { href: "/assignments/reassign", label: "Reassign Assets", icon: ClipboardList, permission: "Assignments.Edit" },
  { href: "/assignments/overdue", label: "Overdue Assignments", icon: ClipboardList, permission: "Assignments.View" },
  { href: "/assignments/history", label: "Assignment History", icon: ClipboardList, permission: "Assignments.View" },
  { href: "/asset-items/transfer", label: "Transfer Assets", icon: ClipboardList, permission: "AssetItems.Edit" },
  { href: "/employees", label: "Employees", icon: Users, permission: "Employees.View" },
  { href: "/locations", label: "Locations", icon: Settings, permission: "Locations.View" },
  { href: "/categories", label: "Categories", icon: Settings, permission: "Categories.View" },
  { href: "/projects", label: "Projects", icon: Settings, permission: "Projects.View" },
  { href: "/vendors", label: "Vendors", icon: Settings, permission: "Vendors.View" },
  { href: "/purchase-orders", label: "Purchase Orders", icon: ClipboardList, permission: "PurchaseOrders.View" },
  { href: "/maintenance-records", label: "Maintenance", icon: ClipboardList, permission: "MaintenanceRecords.View" },
  { href: "/directorates", label: "Directorates", icon: Settings, permission: "Directorates.View" },
  { href: "/reports", label: "Reports", icon: FileBarChart2, permission: "Reports.View" },
  { href: "/admin/roles", label: "Roles", icon: Users, permission: "Roles.View" },
  { href: "/admin/permissions", label: "Permissions", icon: Users, permission: "Permissions.View" },
  { href: "/admin/user-access", label: "User Access", icon: Users, permission: "UserAccess.View" }
];
