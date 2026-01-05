 namespace AssetManagement.Application.DTOs;

 public class EmployeeAssetValueSummaryDto
 {
    public int EmployeeId { get; set; }
     public string EmployeeName { get; set; } = string.Empty;
     public decimal TotalAssetValue { get; set; }

}