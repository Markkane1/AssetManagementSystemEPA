using AssetManagement.Domain.Entities;
using AssetManagement.Domain.ValueObjects;
using Xunit;

namespace AssetManagement.Tests.Domain;

public class AssetTests
{
    [Fact]
    public void CreateAsset_WithValidData_ShouldSucceed()
    {
        // Arrange
        var name = "Laptop Dell XPS";
        var categoryId = 1;
        var acquisitionDate = DateTime.UtcNow;
        var price = Money.Create(1500, "USD");
        var quantity = 5;

        // Act
        var asset = new Asset(name, "CODE123", categoryId, acquisitionDate, price, quantity, quantity, "Manu", "Model", "Spec", null, "Tester", DateTime.UtcNow);

        // Assert
        Assert.Equal(name, asset.Name);
        Assert.Equal(categoryId, asset.CategoryId);
        Assert.Equal(price, asset.Price);
        Assert.Equal(quantity, asset.Quantity);
    }

    [Fact]
    public void CreateAsset_WithEmptyName_ShouldThrowArgumentException()
    {
        // Arrange
        var name = "";
        var price = Money.Create(1000, "USD");

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Asset(name, "C", 1, DateTime.UtcNow, price, 1, 0, "M", "M", "S", null, "T", DateTime.UtcNow));
    }

    [Fact]
    public void CreateAsset_WithUntrackedQuantitySet_ShouldHaveCorrectQuantity()
    {
        // Act
        var asset = new Asset("Laptop", "CODE1", 1, DateTime.UtcNow, Money.Create(1000, "USD"), 10, 10, "M", "M", "S", null, "T", DateTime.UtcNow);

        // Assert
        Assert.Equal(10, asset.Quantity);
        Assert.Equal(10, asset.UntrackedQuantity);
        Assert.Equal(0, asset.TrackedQuantity);
    }

    [Fact]
    public void CreateAsset_InvariantViolation_ShouldThrowException()
    {
        // Act & Assert
        // This is hard to trigger via constructor because it doesn't take TrackedQuantity, 
        // but we can test if the constructor itself enforces sanity if it were possible to bypass.
        // Actually the current constructor takes total and untracked. Tracked is always 0 initially.
        // So total != 0 + untracked would be the violation.
        Assert.Throws<InvalidOperationException>(() => new Asset("Laptop", "CODE1", 1, DateTime.UtcNow, Money.Create(1000, "USD"), 10, 5, "M", "M", "S", null, "T", DateTime.UtcNow));
    }

    [Fact]
    public void AddAssetItem_ShouldDecreaseUntrackedAndIncreaseTracked()
    {
        // Arrange
        var asset = new Asset("Laptop", "CODE1", 1, DateTime.UtcNow, Money.Create(1000, "USD"), 1, 1, "M", "M", "S", null, "T", DateTime.UtcNow);
        var item = new AssetItem { Id = 1, AssetId = asset.Id };

        // Act
        asset.AddAssetItem(item);

        // Assert
        Assert.Equal(1, asset.Quantity);
        Assert.Equal(0, asset.UntrackedQuantity);
        Assert.Equal(1, asset.TrackedQuantity);
        Assert.Contains(item, asset.AssetItems);
    }

    [Fact]
    public void AddAssetItem_WhenUntrackedIsZero_ShouldThrowException()
    {
        // Arrange
        var asset = new Asset("Laptop", "CODE1", 1, DateTime.UtcNow, Money.Create(1000, "USD"), 0, 0, "M", "M", "S", null, "T", DateTime.UtcNow);
        var item = new AssetItem { Id = 1, AssetId = asset.Id };

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => asset.AddAssetItem(item));
    }

    [Fact]
    public void IncreaseUntrackedQuantity_ShouldUpdateTotals()
    {
        // Arrange
        var asset = new Asset("Laptop", "CODE1", 1, DateTime.UtcNow, Money.Create(1000, "USD"), 0, 0, "M", "M", "S", null, "T", DateTime.UtcNow);

        // Act
        asset.IncreaseUntrackedQuantity(5);

        // Assert
        Assert.Equal(5, asset.Quantity);
        Assert.Equal(5, asset.UntrackedQuantity);
    }

    [Fact]
    public void DecreaseUntrackedQuantity_ShouldUpdateTotals()
    {
        // Arrange
        var asset = new Asset("Laptop", "CODE1", 1, DateTime.UtcNow, Money.Create(1000, "USD"), 10, 10, "M", "M", "S", null, "T", DateTime.UtcNow);

        // Act
        asset.DecreaseUntrackedQuantity(3);

        // Assert
        Assert.Equal(7, asset.Quantity);
        Assert.Equal(7, asset.UntrackedQuantity);
    }

    [Fact]
    public void DecreaseUntrackedQuantity_ExceedingAvailable_ShouldThrowException()
    {
        // Arrange
        var asset = new Asset("Laptop", "CODE1", 1, DateTime.UtcNow, Money.Create(1000, "USD"), 5, 5, "M", "M", "S", null, "T", DateTime.UtcNow);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => asset.DecreaseUntrackedQuantity(10));
    }
}
