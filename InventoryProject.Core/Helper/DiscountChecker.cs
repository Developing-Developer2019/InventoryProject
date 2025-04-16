using InventoryProject.Core.Enum;
using InventoryProject.Core.Model;

namespace InventoryProject.Core.Helper;

public static class DiscountChecker
{
    public static (Item item, DiscountAmount? discount) ApplyDiscount(Item item, DateTime? dateTimeInput = null)
    {
        dateTimeInput ??= DateTime.Now;
        DateTime currentTime = (DateTime)dateTimeInput;
        DiscountAmount? discountEnum;
        var count = item.Variations.Sum(v => v.Quantity);

        if (currentTime.DayOfWeek == DayOfWeek.Monday &&
            currentTime.TimeOfDay >= TimeSpan.FromHours(12) &&
            currentTime.TimeOfDay < TimeSpan.FromHours(17))
        {
            discountEnum = DiscountAmount.Fifty;
        }
        else
        {
            discountEnum = count switch
            {
                > 10 => DiscountAmount.Twenty,
                > 5 => DiscountAmount.Ten,
                _ => DiscountAmount.None
            };
        }

        var discountDecimal = (decimal)discountEnum.Value / 100;
        item.Price = Math.Round(item.Price * (1 - discountDecimal), 2);

        return (item, discountEnum);
    }
}
