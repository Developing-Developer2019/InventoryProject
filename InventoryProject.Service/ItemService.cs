using InventoryProject.Data.Data;
using InventoryProject.Service.Interface;

namespace InventoryProject.Service;

public class ItemService : IItemService
{
    private readonly DataContext _dataContext;

    public ItemService(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    
}
