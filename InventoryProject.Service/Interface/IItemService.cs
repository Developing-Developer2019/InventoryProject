using InventoryProject.Core.Model.API;

namespace InventoryProject.Service.Interface;

public interface IItemService
{
    Task<List<ItemResponse>> GetAllItemsAsync();
}
