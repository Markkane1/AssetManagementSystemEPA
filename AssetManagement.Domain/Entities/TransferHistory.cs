using System.ComponentModel.DataAnnotations;

namespace AssetManagement.Domain.Entities;


public class TransferHistory
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Asset item ID is required.")]
    public int AssetItemId { get; set; }
    public AssetItem AssetItem { get; set; } = null!;
    [Required(ErrorMessage = "From location ID is required.")]
    public int FromLocationId { get; set; }
    public Location FromLocation { get; set; } = null!;
    [Required(ErrorMessage = "To location ID is required.")]
    public int ToLocationId { get; set; }
    public Location ToLocation { get; set; } = null!;
    [Required(ErrorMessage = "Transfer date is required.")]
    public DateTime TransferDate { get; set; }
}