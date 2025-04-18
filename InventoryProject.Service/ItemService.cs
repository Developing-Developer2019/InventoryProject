using InventoryProject.Core.Enum;
using InventoryProject.Core.Helper;
using InventoryProject.Core.Model;
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
    
    public async Task<ItemResponse?> GetItemByIdAsync(Guid id)
    {
        var item = await _dataContext.Items.Where(item => item.Id == id)
            .Include(variation => variation.Variations)
            .FirstOrDefaultAsync();

        if (item == null)
        {
            return null;
        }
        
        (var discountedItem, DiscountAmount? discount) = DiscountChecker.ApplyDiscount(item);
        return discountedItem.MapToItemResponse(discount);
    }
    
    public async Task<ItemResponse?> CreateItemAsync(Item item)
    {
        _dataContext.Items.Add(item);
        var saved = await _dataContext.SaveChangesAsync() > 0;
        if (!saved) return null;

        (var discountedItem, DiscountAmount? discount) = DiscountChecker.ApplyDiscount(item);
        return discountedItem.MapToItemResponse(discount);
    }
    
    public async Task<bool> UpdateItemAsync(ItemUpdateRequest update)
    {
        var existing = await _dataContext.Items
            .Include(i => i.Variations)
            .FirstOrDefaultAsync(i => i.Id == update.Id);

        if (existing == null) return false;
        
        if (update.Reference != null) existing.Reference = update.Reference;
        if (update.Name != null) existing.Name = update.Name;
        if (update.Price.HasValue) existing.Price = update.Price.Value;
        if (update.Variations != null) existing.Variations = update.Variations;

        var result = await _dataContext.SaveChangesAsync();
        return result > 0;
    }
    
    public async Task<bool> DeleteItemAsync(Guid id)
    {
        var item = await _dataContext.Items.Where(item => item.Id == id).FirstOrDefaultAsync();
        if (item == null) return false;
        _dataContext.Items.Remove(item);
        var result = await _dataContext.SaveChangesAsync();
        return result > 0;
    }
}
