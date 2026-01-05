using System.ComponentModel.DataAnnotations;

namespace AssetManagement.Domain.Entities;

public class Location
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Name is required.")]
    [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Address is required.")]
    [StringLength(200, ErrorMessage = "Address cannot exceed 200 characters.")]
    public string Address { get; set; } = string.Empty;

    public ICollection<AssetItem> AssetItems { get; set; } = new List<AssetItem>();
    public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    public ICollection<Directorate> Directorates { get; set; } = new List<Directorate>();
}