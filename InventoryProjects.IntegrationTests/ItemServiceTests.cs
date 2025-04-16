using InventoryProject.Core.Model;
using InventoryProject.Core.Model.API;
using InventoryProject.Data.Data;
using InventoryProject.Service;
using InventoryProject.Service.Interface;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace InventoryProjects.IntegrationTests;

[TestFixture]
public class ItemServiceTests
{
    private DataContext _dataContext;
    private IItemService _itemService;

    /// <summary>
    /// Here I am using the SqlLite data, when in reality, there wouldn't be Database.EnsureCreated(); in DataContext.cs
    /// So it would be mock data being created.
    /// </summary>
    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _dataContext = new DataContext(options);
        _dataContext.Database.EnsureCreated();
        _itemService = new ItemService(_dataContext);
    }

    [Test]
    public async Task GetAllItemsAsync_ShouldReturnItems()
    {
        var items = await _itemService.GetAllItemsAsync();
        Assert.That(items, Is.Not.Empty);
        Assert.That(items.First().Name, Is.EqualTo("Shorts"));
        Assert.That(items.First().Variations.Count, Is.EqualTo(3));
    }
    
    [Test]
    public async Task GetItemByIdAsync_ShouldReturnCorrectItem()
    {
        var shorts = _dataContext.Items.Include(i => i.Variations).First(i => i.Reference == "A123");
        var result = await _itemService.GetItemByIdAsync(shorts.Id);

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Reference, Is.EqualTo("A123"));
        Assert.That(result.Name, Is.EqualTo("Shorts"));
        Assert.That(result.Variations.Count, Is.EqualTo(3));
        Assert.That(result.Variations.Sum(v => v.Quantity), Is.EqualTo(10));
    }

    [Test]
    public async Task CreateItemAsync_ShouldCreateItem()
    {
        var itemId = new Guid("24b078a2-9bd0-4cf7-91f1-796161fd8e7b");

        Item item = new Item()
        {
            Id = itemId,
            Reference = "Sample Reference",
            Name = "Sample Item",
            Price = 10,
            Variations = new List<Variation>()
            {
                new Variation() { Id = Guid.NewGuid(), ItemId = itemId, Quantity = 5, Size = "Small" },
                new Variation() { Id = Guid.NewGuid(), ItemId = itemId, Quantity = 2, Size = "Medium" }
            }
        };
        
        var result = await _itemService.CreateItemAsync(item);
        
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Variations.Count, Is.EqualTo(2));
    }
    
    [Test]
    public async Task UpdateItemAsync_ShouldUpdateItem()
    {
        // Arrange
        var itemId = Guid.NewGuid();

        var item = new Item
        {
            Id = itemId,
            Reference = "OldRef",
            Name = "OldName",
            Price = 5
        };

        _dataContext.Items.Add(item);
        await _dataContext.SaveChangesAsync();

        var updateRequest = new ItemUpdateRequest
        {
            Id = itemId,
            Reference = "NewRef",
            Name = "NewName",
            Price = 20
        };

        var success = await _itemService.UpdateItemAsync(updateRequest);

        // Assert
        Assert.That(success, Is.True);

        var updated = await _dataContext.Items.Include(i => i.Variations).FirstOrDefaultAsync(i => i.Id == itemId);

        Assert.That(updated, Is.Not.Null);
        Assert.That(updated!.Reference, Is.EqualTo("NewRef"));
        Assert.That(updated.Name, Is.EqualTo("NewName"));
        Assert.That(updated.Price, Is.EqualTo(20));
    }
}
