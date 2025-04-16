using InventoryProject.Core.Enum;
using InventoryProject.Core.Helper;
using InventoryProject.Core.Model;

namespace InventoryProject.Tests;

[TestFixture]
public class ItemMapperTests
{
    [Test]
    public void MapToItemResponse_MapsAllFieldsCorrectly()
    {
        // Arrange
        var itemId = Guid.NewGuid();
        var variations = new List<Variation>
        {
            new Variation { Id = Guid.NewGuid(), Size = "L", Quantity = 2 }
        };

        var item = new Item
        {
            Id = itemId,
            Reference = "ABC123",
            Name = "Test Item",
            Price = 99.99m,
            Variations = variations
        };

        var discount = DiscountAmount.Twenty;

        // Act
        var response = item.MapToItemResponse(discount);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(response.Id, Is.EqualTo(item.Id));
            Assert.That(response.Reference, Is.EqualTo(item.Reference));
            Assert.That(response.Name, Is.EqualTo(item.Name));
            Assert.That(response.Price, Is.EqualTo(item.Price));
            Assert.That(response.Variations, Is.EqualTo(item.Variations));
            Assert.That(response.DiscountAmount, Is.EqualTo(discount));
        });
    }
}
