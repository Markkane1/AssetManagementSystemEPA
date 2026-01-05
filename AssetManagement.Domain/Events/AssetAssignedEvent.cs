using AssetManagement.Domain.Common;

namespace AssetManagement.Domain.Events;

public class AssetAssignedEvent : IDomainEvent
{
    public int AssetId { get; }
    public int EmployeeId { get; }
    public DateTime AssignedOn { get; }

    public AssetAssignedEvent(int assetId, int employeeId)
    {
        AssetId = assetId;
        EmployeeId = employeeId;
        AssignedOn = DateTime.UtcNow;
    }

    public DateTime OccurredOn => AssignedOn;
}
