 namespace AssetManagement.Application.DTOs;

 public class LocationTransferSummaryDto
 {
    public int FromLocationId { get; set; }
     public int ToLocationId { get; set; }
     public int TransferCount { get; set; }
     public decimal TotalValue { get; set; }
}