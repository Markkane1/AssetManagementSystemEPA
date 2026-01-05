using System.ComponentModel.DataAnnotations;

namespace AssetManagement.Domain.Entities;

public class Directorate
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Name is required.")]
    [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
    public string Name { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Location ID is required.")]
    public int LocationId { get; set; }

    public Location Location { get; set; } = null!;

    public ICollection<Employee> Employees { get; set; } = new List<Employee>();
}