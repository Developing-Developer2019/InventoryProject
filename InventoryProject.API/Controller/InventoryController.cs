using InventoryProject.Core.Model;
using Microsoft.AspNetCore.Mvc;

namespace InventoryProject.API.Controller;

[ApiController]
[Route("api/[controller]")]
public class InventoryController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllItems()
    {
        return Ok(new List<Item>());
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
