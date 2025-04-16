using InventoryProject.Core.Model;
using InventoryProject.Core.Model.API;
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
    public async Task<IActionResult> UpdateItem(ItemUpdateRequest itemUpdateRequest)
    {
        if (itemUpdateRequest.Id == Guid.Empty)
        {
            return BadRequest(new ProblemDetails
            {
                Title = "Id is required",
                Status = StatusCodes.Status400BadRequest
            });
        }

        if (itemUpdateRequest.Reference == null && itemUpdateRequest.Name == null && itemUpdateRequest.Price == null && itemUpdateRequest.Variations == null)
        {
            return BadRequest(new ProblemDetails
            {
                Title = "At least one field to update must be provided",
                Status = StatusCodes.Status400BadRequest
            });
        }

        var result = await _itemService.UpdateItemAsync(itemUpdateRequest);

        if (!result)
        {
            return BadRequest(new ProblemDetails
            {
                Title = "Unable to update item",
                Status = StatusCodes.Status400BadRequest
            });
        }

        return Ok("Item updated");
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteItem(Guid id)
    {
        var result = await _itemService.DeleteItemAsync(id);
        
        if (!result)
        {
            return BadRequest(new ProblemDetails
            {
                Title = "Unable to delete item",
                Status = StatusCodes.Status400BadRequest,
                Detail = $"Cannot delete item with ID {id}"
            });
        }
        
        return Ok("Item deleted");
    }
}
