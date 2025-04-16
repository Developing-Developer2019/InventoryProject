using InventoryProject.API.Controller;
using InventoryProject.Core.Enum;
using InventoryProject.Core.Helper;
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
        var mockItems = CreateItemResponses();

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
        var mockItems = CreateItemResponses();
        var itemId = Guid.NewGuid();

        _mockService.Setup(s => s.GetItemByIdAsync(itemId)).ReturnsAsync(mockItems.First);

        var result = await _controller.GetItemById(itemId) as OkObjectResult;

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
        var itemId = Guid.NewGuid();
        _mockService.Setup(s => s.GetAllItemsAsync()).ReturnsAsync(new List<ItemResponse>());

        var result =
            await _controller.GetItemById(itemId) as NotFoundObjectResult;

        // Assert
        Assert.That(result, Is.Not.Null);

        var problem = result.Value as ProblemDetails;
        Assert.That(problem, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(problem.Status, Is.EqualTo(StatusCodes.Status404NotFound));
            Assert.That(problem.Title, Is.EqualTo("Item not found"));
            Assert.That(problem.Detail, Is.EqualTo($"No item found with ID {itemId}"));
        });
    }
    
    [Test]
    public async Task CreateItem_WhenSuccessful_ReturnsCreatedResult()
    {
        var item = CreateItem();
        var response = item.MapToItemResponse(DiscountAmount.None);

        _mockService.Setup(s => s.CreateItemAsync(It.IsAny<Item>()))
            .ReturnsAsync(response);

        var result = await _controller.CreateItem(item);
        var created = result as CreatedAtActionResult;

        Assert.That(created, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(created!.StatusCode, Is.EqualTo(StatusCodes.Status201Created));
            Assert.That(created.Value, Is.EqualTo(response.Id));
        });
    }

    [Test]
    public async Task CreateItem_WhenUnsuccessful_ReturnsBadRequest()
    {
        var item = CreateItem();

        _mockService.Setup(s => s.CreateItemAsync(It.IsAny<Item>()))
            .ReturnsAsync((ItemResponse?)null); // simulate failure

        var result = await _controller.CreateItem(item);
        var badRequest = result as BadRequestObjectResult;

        Assert.That(badRequest, Is.Not.Null);
        Assert.That(badRequest!.StatusCode, Is.EqualTo(StatusCodes.Status400BadRequest));
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

    private List<ItemResponse> CreateItemResponses()
    {
        var itemIdOne = Guid.NewGuid();
        var itemIdTwo = Guid.NewGuid();

        return new List<ItemResponse>
        {
            new ItemResponse
            {
                Id = itemIdOne,
                Name = "One",
                Reference = "ZX321",
                Price = 10,
                DiscountAmount = DiscountAmount.None,
                Variations = new List<Variation>
                {
                    new Variation() { Id = Guid.NewGuid(), ItemId = itemIdOne, Quantity = 5, Size = "Small" },
                    new Variation() { Id = Guid.NewGuid(), ItemId = itemIdOne, Quantity = 2, Size = "Medium" }
                }
            },
            new ItemResponse
            {
                Id = itemIdTwo,
                Name = "Two",
                Reference = "ZX321",
                Price = 8,
                DiscountAmount = DiscountAmount.None,
                Variations = new List<Variation>
                {
                    new Variation() { Id = Guid.NewGuid(), ItemId = itemIdOne, Quantity = 3, Size = "Small" },
                }
            }
        };
    }

    private Item CreateItem()
    {
        var itemIdOne = Guid.NewGuid();

        return new Item
        {
            Id = itemIdOne,
            Name = "One",
            Reference = "ZX321",
            Price = 10,
            Variations = new List<Variation>
            {
                new Variation() { Id = Guid.NewGuid(), ItemId = itemIdOne, Quantity = 1, Size = "Small" },
                new Variation() { Id = Guid.NewGuid(), ItemId = itemIdOne, Quantity = 2, Size = "Medium" }
            }
        };
    }
}
