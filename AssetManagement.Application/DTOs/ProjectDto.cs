using System.ComponentModel.DataAnnotations;

namespace AssetManagement.Application.DTOs
{
    public record ProjectDto(
        int Id,
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        string Name,
        [StringLength(500, ErrorMessage = "Project description cannot exceed 500 characters.")]
        string ProjectDescription,
        [Required(ErrorMessage = "Project run by is required.")]
        [StringLength(100, ErrorMessage = "Project run by cannot exceed 100 characters.")]
        string ProjectRunBy);
}