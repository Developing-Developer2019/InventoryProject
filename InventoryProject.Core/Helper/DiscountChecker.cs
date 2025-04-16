using InventoryProject.Core.Enum;
using InventoryProject.Core.Model;

namespace InventoryProject.Core.Helper;

public static class DiscountChecker
{
    public static (Item item, DiscountAmount? discount) ApplyDiscount(Item item)
    {
        var date = DateTime.Now;
        DiscountAmount? discountEnum;
        var count = item.Variations.Sum(v => v.Quantity);

        if (date.DayOfWeek == DayOfWeek.Monday &&
            date.TimeOfDay >= TimeSpan.FromHours(12) &&
            date.TimeOfDay < TimeSpan.FromHours(17))
        {
            discountEnum = DiscountAmount.Fifty;
        }
        else
        {
            discountEnum = count switch
            {
                > 10 => DiscountAmount.Twenty,
                > 5 => DiscountAmount.Ten,
                _ => null
            };
        }

        if (discountEnum == null) 
            return (item, discountEnum);
        
        var discountDecimal = (decimal)discountEnum.Value / 100;
        item.Price = Math.Round(item.Price * (1 - discountDecimal), 2);

        return (item, discountEnum);
    }
}
