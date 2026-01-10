using AssetManagement.Domain.Common;

namespace AssetManagement.Domain.Entities;

public class UserLocationAccess : Entity
{
    public string UserId { get; private set; }
    public int LocationId { get; private set; }
    public Location Location { get; private set; } = null!;

    // Required for EF Core
    protected UserLocationAccess()
    {
        UserId = null!;
    }

    public UserLocationAccess(string userId, int locationId)
    {
        if (string.IsNullOrWhiteSpace(userId))
            throw new ArgumentException("User ID cannot be empty.", nameof(userId));
        if (locationId <= 0)
            throw new ArgumentException("Location ID must be valid.", nameof(locationId));

        UserId = userId;
        LocationId = locationId;
    }
}
