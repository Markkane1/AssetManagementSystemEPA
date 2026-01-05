using AssetManagement.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace AssetManagement.Application.DTOs
{
    public class AssetItemDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Asset ID is required.")]
        public int AssetId { get; set; }
        [Required(ErrorMessage = "Location ID is required.")]
        public int LocationId { get; set; }
        [Required(ErrorMessage = "Serial number is required.")]
        [StringLength(50, ErrorMessage = "Serial number cannot exceed 50 characters.")]
        public string SerialNumber { get; set; } = string.Empty;
        [Required(ErrorMessage = "Tag is required.")]
        [StringLength(10, MinimumLength = 6, ErrorMessage = "Tag must be 6 to 10 digits.")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "Tag must contain only digits.")]
        public string Tag { get; set; } = string.Empty;
        public AssignmentStatus AssignmentStatus { get; set; }
        [Required(ErrorMessage = "Item status is required.")]
        public ItemStatus Status { get; set; }
        [Required(ErrorMessage = "Item source is required.")]
        public ItemSource Source { get; set; }

        public AssetItemDto(int id, int assetId, int locationId, string serialNumber, string tag, AssignmentStatus assignmentStatus)
        {
            Id = id;
            AssetId = assetId;
            LocationId = locationId;
            SerialNumber = serialNumber;
            Tag = tag;
            AssignmentStatus = assignmentStatus;
        }

        public AssetItemDto(int id, int assetId, int locationId, string serialNumber, string tag,
            AssignmentStatus assignmentStatus, ItemStatus status, ItemSource source)
        {
            Id = id;
            AssetId = assetId;
            LocationId = locationId;
            SerialNumber = serialNumber;
            Tag = tag;
            AssignmentStatus = assignmentStatus;
            Status = status;
            Source = source;
        }
    }
}
