using InventoryProject.Core.Enum;
using InventoryProject.Core.Helper;
using InventoryProject.Core.Model;

namespace InventoryProject.Tests;

[TestFixture]
public class DiscountCheckerTests
{
    [TestCase(DayOfWeek.Monday, 11, 59, 10, DiscountAmount.Ten, 90)]
    [TestCase(DayOfWeek.Monday, 12, 01, 100, DiscountAmount.Fifty, 50)]
    [TestCase(DayOfWeek.Monday, 16, 59, 100, DiscountAmount.Fifty, 50)]
    [TestCase(DayOfWeek.Monday, 17, 01, 10, DiscountAmount.Ten, 90)]
    [TestCase(DayOfWeek.Tuesday, 0, 0, 11, DiscountAmount.Twenty, 80)]
    [TestCase(DayOfWeek.Wednesday, 0, 0, 6, DiscountAmount.Ten, 90)]
    [TestCase(DayOfWeek.Friday, 0, 0, 3, DiscountAmount.None, 100)]
    public void ApplyDiscount_ReturnsExpectedDiscount(
        DayOfWeek day, int hour, int minute, int quantity,
        DiscountAmount? expectedDiscount, decimal expectedPrice)
    {
        // Arrange
        var item = CreateItem(quantity);
        var testDate = GetNextWeekday(day, hour, minute);
        
        
        var result = ApplyDiscountWithMockedTime(item, testDate);
        
        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(result.discount, Is.EqualTo(expectedDiscount));
            Assert.That(result.item.Price, Is.EqualTo(expectedPrice));
        });
    }

    private static (Item item, DiscountAmount? discount) ApplyDiscountWithMockedTime(Item item, DateTime testDateTime)
    {
        return DiscountChecker.ApplyDiscount(item, testDateTime);
    }
    
    private static Item CreateItem(int quantity, decimal price = 100)
    {
        return new Item
        {
            Price = price,
            Variations = new List<Variation>
            {
                new Variation { Quantity = quantity }
            }
        };
    }
    
    private static DateTime GetNextWeekday(DayOfWeek targetDay, int hour, int minute)
    {
        var date = new DateTime(2025, 1, 1);
        while (date.DayOfWeek != targetDay)
            date = date.AddDays(1);
        return new DateTime(date.Year, date.Month, date.Day, hour, minute, 0);
    }
}