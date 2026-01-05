using AssetManagement.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace AssetManagement.Domain.Entities;

public class AssetItem
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Asset ID is required.")]
        public int AssetId { get; set; }

        public Asset Asset { get; set; } = null!;

        [Required(ErrorMessage = "Location ID is required.")]
        public int LocationId { get; set; }

        public Location Location { get; set; } = null!;

        [Required(ErrorMessage = "Serial number is required.")]
        [StringLength(50, ErrorMessage = "Serial number cannot exceed 50 characters.")]
        public string SerialNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Tag is required.")]
        [StringLength(10, MinimumLength = 6, ErrorMessage = "Tag must be 6 to 10 digits.")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "Tag must contain only digits.")]
        public string Tag { get; set; } = string.Empty;

        public AssignmentStatus AssignmentStatus { get; set; }

        public ItemStatus Status { get; set; } = ItemStatus.Functional; // Default to Functional
        public ItemSource Source { get; set; }

    public ICollection<Assignment> Assignments { get; set; } = new List<Assignment>();
        public ICollection<MaintenanceRecord> MaintenanceRecords { get; set; } = new List<MaintenanceRecord>();

        public ICollection<TransferHistory> TransferHistories { get; set; } = new List<TransferHistory>();
}