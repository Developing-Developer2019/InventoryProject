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
        var item = await _itemService.GetItemByIdAsync(id);
        
        if (item == null)
            return NotFound(new ProblemDetails
            {
                Title = "Item not found",
                Status = StatusCodes.Status404NotFound,
                Detail = $"No item found with ID {id}"
            });
        
        return Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> CreateItem(Item item)
    {
        var result = await _itemService.CreateItemAsync(item);

        if (result == null)
        {
            return BadRequest(new ProblemDetails
            {
                Title = "Unable to create item",
                Status = StatusCodes.Status400BadRequest
            });
        }

        return CreatedAtAction(nameof(GetItemById), new { id = result.Id }, result.Id);
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
