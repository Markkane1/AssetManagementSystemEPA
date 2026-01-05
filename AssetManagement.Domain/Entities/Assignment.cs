using System.ComponentModel.DataAnnotations;

namespace AssetManagement.Domain.Entities;

public class Assignment
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Asset item ID is required.")]
    public int AssetItemId { get; set; }

    public AssetItem AssetItem { get; set; } = null!;

    [Required(ErrorMessage = "Employee ID is required.")]
    public int EmployeeId { get; set; }

    public Employee Employee { get; set; } = null!;

    [Required(ErrorMessage = "Assignment date is required.")]
    public DateTime AssignmentDate { get; set; }

    public DateTime? ReturnDate { get; set; }
}