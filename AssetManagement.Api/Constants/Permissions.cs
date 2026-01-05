namespace AssetManagement.Api.Constants;

public static class Permissions
{
    public static class Assets
    {
        public const string Read = "Assets.Read";
        public const string Create = "Assets.Create";
        public const string Update = "Assets.Update";
        public const string Delete = "Assets.Delete";
    }

    public static class AssetItems
    {
        public const string Read = "AssetItems.Read";
        public const string Create = "AssetItems.Create";
        public const string Update = "AssetItems.Update";
        public const string Delete = "AssetItems.Delete";
        public const string Transfer = "AssetItems.Transfer";
    }

    public static class Assignments
    {
        public const string Read = "Assignments.Read";
        public const string Create = "Assignments.Create";
        public const string Update = "Assignments.Update";
        public const string Delete = "Assignments.Delete";
        public const string Return = "Assignments.Return";
        public const string Reassign = "Assignments.Reassign";
    }

    public static class Reports
    {
        public const string View = "Reports.View";
    }

    public static class Categories
    {
        public const string Read = "Categories.Read";
        public const string Create = "Categories.Create";
        public const string Update = "Categories.Update";
        public const string Delete = "Categories.Delete";
    }

    public static class Directorates
    {
        public const string Read = "Directorates.Read";
        public const string Create = "Directorates.Create";
        public const string Update = "Directorates.Update";
        public const string Delete = "Directorates.Delete";
    }

    public static class Employees
    {
        public const string Read = "Employees.Read";
        public const string Create = "Employees.Create";
        public const string Update = "Employees.Update";
        public const string Delete = "Employees.Delete";
    }

    public static class Locations
    {
        public const string Read = "Locations.Read";
        public const string Create = "Locations.Create";
        public const string Update = "Locations.Update";
        public const string Delete = "Locations.Delete";
    }

    public static class Maintenance
    {
        public const string Read = "Maintenance.Read";
        public const string Create = "Maintenance.Create";
        public const string Update = "Maintenance.Update";
        public const string Delete = "Maintenance.Delete";
    }

    public static class Projects
    {
        public const string Read = "Projects.Read";
        public const string Create = "Projects.Create";
        public const string Update = "Projects.Update";
        public const string Delete = "Projects.Delete";
    }

    public static class PurchaseOrders
    {
        public const string Read = "PurchaseOrders.Read";
        public const string Create = "PurchaseOrders.Create";
        public const string Update = "PurchaseOrders.Update";
        public const string Delete = "PurchaseOrders.Delete";
    }

    public static class Vendors
    {
        public const string Read = "Vendors.Read";
        public const string Create = "Vendors.Create";
        public const string Update = "Vendors.Update";
        public const string Delete = "Vendors.Delete";
    }

    public static class UserManagement
    {
        public const string ManagePermissions = "UserManagement.ManagePermissions";
        public const string ViewUsers = "UserManagement.ViewUsers";
    }

    // Helper method to get all permission constants
    public static List<string> GetAll()
    {
        var permissions = new List<string>();
        var types = typeof(Permissions).GetNestedTypes();

        foreach (var type in types)
        {
            var fields = type.GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
            foreach (var field in fields)
            {
                if (field.FieldType == typeof(string))
                {
                    var value = field.GetValue(null)?.ToString();
                    if (value != null)
                        permissions.Add(value);
                }
            }
        }

        return permissions;
    }
}
