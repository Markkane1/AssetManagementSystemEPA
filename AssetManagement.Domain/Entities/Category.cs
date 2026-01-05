using System.ComponentModel.DataAnnotations;

namespace AssetManagement.Domain.Entities;

public class Category
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Name is required.")]
    [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
    public string Name { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
    public string Description { get; set; } = string.Empty;

    public ICollection<Asset> Assets { get; set; } = new List<Asset>();
}