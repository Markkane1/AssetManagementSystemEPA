using System.ComponentModel.DataAnnotations;
using AssetManagement.Domain.Enums;

namespace AssetManagement.Domain.Entities;

public class MaintenanceRecord
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Asset item ID is required.")]
    public int AssetItemId { get; set; }

    public AssetItem AssetItem { get; set; } = null!;

    [Required(ErrorMessage = "Maintenance date is required.")]
    public DateTime MaintenanceDate { get; set; }

    [Required(ErrorMessage = "Description is required.")]
    [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
    public string Description { get; set; } = string.Empty;

    [Range(0, double.MaxValue, ErrorMessage = "Cost cannot be negative.")]
    public decimal Cost { get; set; }

    public MaintenanceStatus Status { get; set; }
}