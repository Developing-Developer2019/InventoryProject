using InventoryProject.Core.Model;
using InventoryProject.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace InventoryProject.API.Controller;

[ApiController]
[Route("api/[controller]")]
public class InventoryController : ControllerBase
{
    private readonly IItemService _itemService;

    public InventoryController(IItemService itemService)
    {
        _itemService = itemService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllItems()
    {
        var items = await _itemService.GetAllItemsAsync();

        
        if (!items.Any())
        {
            return NotFound(new ProblemDetails
            {
                Title = "No items found",
                Status = StatusCodes.Status404NotFound,
                Detail = "No items found"
            });
        }
        
        return Ok(await _itemService.GetAllItemsAsync());
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetItemById(Guid id)
    {
        return Ok(new Item());
    }

    [HttpPost]
    public async Task<IActionResult> CreateItem(Item item)
    {
        return Ok("Item created");
    }

    [HttpPatch]
    public async Task<IActionResult> UpdateItem(Item item)
    {
        return Ok("Item updated");
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteItem(Guid id)
    {
        return Ok("Item deleted");
    }
}
