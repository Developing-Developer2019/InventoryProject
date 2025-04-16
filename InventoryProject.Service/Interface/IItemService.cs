using InventoryProject.Core.Model;
using InventoryProject.Core.Model.API;

namespace InventoryProject.Service.Interface;

public interface IItemService
{
    Task<List<ItemResponse>> GetAllItemsAsync();
    Task<ItemResponse?> GetItemByIdAsync(Guid id);
    Task<ItemResponse?> CreateItemAsync(Item item);
    Task<bool> UpdateItemAsync(ItemUpdateRequest item);
    Task<bool> DeleteItemAsync(Guid id);
}
