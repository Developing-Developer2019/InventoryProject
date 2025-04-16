using InventoryProject.Core.Enum;
using InventoryProject.Core.Model;
using InventoryProject.Core.Model.API;

namespace InventoryProject.Core.Helper;

public static class ItemMapper
{
    public static ItemResponse MapToItemResponse(this Item item, DiscountAmount? discount)
    {
        return new ItemResponse
        {
            Id = item.Id,
            Reference = item.Reference,
            Name = item.Name,
            Price = item.Price,
            Variations = item.Variations,
            DiscountAmount = discount
        };
    }
}
