using AssetManagement.Domain.Common;
using AssetManagement.Domain.ValueObjects;

namespace AssetManagement.Domain.Entities;

public class Employee : Entity, IAggregateRoot
{
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public Email Email { get; private set; }
    public string Role { get; private set; }
    public int DirectorateId { get; private set; }
    public Directorate Directorate { get; private set; } = null!;
    public int LocationId { get; private set; }
    public Location Location { get; private set; } = null!;

    private readonly List<Assignment> _assignments = new();
    public IReadOnlyCollection<Assignment> Assignments => _assignments.AsReadOnly();

    protected Employee()
    {
        FirstName = null!;
        LastName = null!;
        Email = null!;
        Role = null!;
    }

    public Employee(string firstName, string lastName, Email email, string role, int directorateId, int locationId)
    {
        if (string.IsNullOrWhiteSpace(firstName)) throw new ArgumentException("First name is required.", nameof(firstName));
        if (string.IsNullOrWhiteSpace(lastName)) throw new ArgumentException("Last name is required.", nameof(lastName));
        if (string.IsNullOrWhiteSpace(role)) throw new ArgumentException("Role is required.", nameof(role));
        if (directorateId <= 0) throw new ArgumentException("Invalid Directorate ID.", nameof(directorateId));
        if (locationId <= 0) throw new ArgumentException("Invalid Location ID.", nameof(locationId));

        FirstName = firstName;
        LastName = lastName;
        Email = email ?? throw new ArgumentNullException(nameof(email));
        Role = role;
        DirectorateId = directorateId;
        LocationId = locationId;
    }

    public void UpdateRole(string newRole)
    {
        if (string.IsNullOrWhiteSpace(newRole)) throw new ArgumentException("Role cannot be empty.", nameof(newRole));
        Role = newRole;
    }

    public void AssignAsset(Assignment assignment)
    {
        ArgumentNullException.ThrowIfNull(assignment);
        _assignments.Add(assignment);
    }
}