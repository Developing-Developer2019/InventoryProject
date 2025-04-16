using InventoryProject.API.Controller;
using InventoryProject.Core.Model;
using InventoryProject.Core.Model.API;
using InventoryProject.Service;
using InventoryProject.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace InventoryProject.Tests;

[TestFixture]
public class InventoryControllerTests
{
    private InventoryController _controller;
    private Mock<IItemService> _mockService;

    [SetUp]
    public void Setup()
    {
        _mockService = new Mock<IItemService>();
        _controller = new InventoryController(_mockService.Object);
    }

    [Test]
    public async Task GetAllItems_WhenItemsExist_ReturnsOkWithItems()
    {
        var mockItems = new List<ItemResponse>
        {
            new ItemResponse() { Id = Guid.NewGuid(), Name = "One" },
            new ItemResponse() { Id = Guid.NewGuid(), Name = "Two" }
        };
        _mockService.Setup(s => s.GetAllItemsAsync()).ReturnsAsync(mockItems);

        var result = await _controller.GetAllItems() as OkObjectResult;

        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result!.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
            Assert.That(result.Value, Is.EqualTo(mockItems));
        });
    }

    [Test]
    public async Task GetAllItems_WhenNoItemsExist_ReturnsNotFound()
    {
        _mockService.Setup(s => s.GetAllItemsAsync()).ReturnsAsync(new List<ItemResponse>());

        var result = await _controller.GetAllItems() as NotFoundObjectResult;

        // Assert
        Assert.That(result, Is.Not.Null);
    
        var problem = result.Value as ProblemDetails;
        Assert.That(problem, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(problem.Status, Is.EqualTo(StatusCodes.Status404NotFound));
            Assert.That(problem.Title, Is.EqualTo("No items found"));
            Assert.That(problem.Detail, Is.EqualTo("No items found"));
        });
    }

    [Test]
    public async Task GetItemById_WhenItemExists_ReturnsOkWithItem()
    {
        var mockItems = new List<ItemResponse>
        {
            new ItemResponse() { Id = Guid.NewGuid(), Name = "One" },
            new ItemResponse() { Id = new Guid("d9e6b0db-5810-49b2-b3b9-53fba78b836e"), Name = "Two" }
        };
        
        _mockService.Setup(s => s.GetItemByIdAsync(new Guid("d9e6b0db-5810-49b2-b3b9-53fba78b836e"))).ReturnsAsync(mockItems.First);

        var result = await _controller.GetItemById(new Guid("d9e6b0db-5810-49b2-b3b9-53fba78b836e")) as OkObjectResult;

        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result!.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
            Assert.That(result.Value, Is.EqualTo(mockItems.First()));
        });
    }
    
    [Test]
    public async Task GetItemById_WhenNoItemExists_ReturnsNotFound()
    {
        _mockService.Setup(s => s.GetAllItemsAsync()).ReturnsAsync(new List<ItemResponse>());

        var result = await _controller.GetItemById(new Guid("d9e6b0db-5810-49b2-b3b9-53fba78b836e")) as NotFoundObjectResult;

        // Assert
        Assert.That(result, Is.Not.Null);
    
        var problem = result.Value as ProblemDetails;
        Assert.That(problem, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(problem.Status, Is.EqualTo(StatusCodes.Status404NotFound));
            Assert.That(problem.Title, Is.EqualTo("Item not found"));
            Assert.That(problem.Detail, Is.EqualTo("No item found with ID d9e6b0db-5810-49b2-b3b9-53fba78b836e"));
        });
    }

    [Test]
    public async Task CreateItem_ReturnsOk()
    {
        var result = await _controller.CreateItem(new Item());
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
    }

    [Test]
    public async Task UpdateItem_ReturnsOk()
    {
        var result = await _controller.UpdateItem(new Item());
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
    }

    [Test]
    public async Task DeleteItem_ReturnsOk()
    {
        var result = await _controller.DeleteItem(Guid.NewGuid());
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
    }
}