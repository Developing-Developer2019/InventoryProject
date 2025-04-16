using InventoryProject.Core.Enum;
using InventoryProject.Core.Helper;
using InventoryProject.Core.Model.API;
using InventoryProject.Data.Data;
using InventoryProject.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace InventoryProject.Service;

public class ItemService : IItemService
{
    private readonly DataContext _dataContext;

    public ItemService(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<List<ItemResponse>> GetAllItemsAsync()
    {
        var items = await _dataContext.Items
            .Include(item => item.Variations)
            .ToListAsync();
        
        var mappedResponse = new List<ItemResponse>();

        foreach (var item in items)
        {
            (var discountedItem, DiscountAmount? discount) = DiscountChecker.ApplyDiscount(item);
            mappedResponse.Add(discountedItem.MapToItemResponse(discount));
        }

        return mappedResponse;
    }
    
}
