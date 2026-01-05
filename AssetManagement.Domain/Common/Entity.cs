namespace AssetManagement.Domain.Common;

public abstract class Entity
{
    public int Id { get; protected set; }

    protected Entity() { }

    // Core entity logic can go here (equality checks, etc.)
}
