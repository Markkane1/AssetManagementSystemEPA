namespace AssetManagement.Application.DTOs;

public class TransferHistoryDto
{
    public int Id { get; set; }
    public int AssetItemId { get; set; }
    public int FromLocationId { get; set; }
    public int ToLocationId { get; set; }
    public DateTime TransferDate { get; set; }
}