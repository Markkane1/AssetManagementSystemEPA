using System.ComponentModel.DataAnnotations;

namespace AssetManagement.Domain.Entities;

public class Project
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Name is required.")]
    [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
    public string Name { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "Project description cannot exceed 500 characters.")]
    public string ProjectDescription { get; set; } = string.Empty;

    [Required(ErrorMessage = "Project run by is required.")]
    [StringLength(100, ErrorMessage = "Project run by cannot exceed 100 characters.")]
    public string ProjectRunBy { get; set; } = string.Empty;

    public ICollection<Asset> Assets { get; set; } = new List<Asset>();
}