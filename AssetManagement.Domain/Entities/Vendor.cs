using System.ComponentModel.DataAnnotations;

namespace AssetManagement.Domain.Entities;

public class Vendor
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Name is required.")]
    [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Address is required.")]
    [StringLength(200, ErrorMessage = "Address cannot exceed 200 characters.")]
    public string Address { get; set; } = string.Empty;

    [Required(ErrorMessage = "Contact number is required.")]
    [StringLength(20, ErrorMessage = "Contact number cannot exceed 20 characters.")]
    public string ContactNumber { get; set; } = string.Empty;

    public ICollection<PurchaseOrder> PurchaseOrders { get; set; } = new List<PurchaseOrder>();
}