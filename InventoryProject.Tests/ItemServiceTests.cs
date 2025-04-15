using InventoryProject.Service;
using InventoryProject.Data.Data;
using Microsoft.EntityFrameworkCore;

namespace InventoryProject.Tests;

[TestFixture]
public class ItemServiceTests
{
    private DataContext _context;
    private ItemService _service;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName: "DummyDb")
            .Options;

        _context = new DataContext(options);
        _service = new ItemService(_context);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Test]
    public void Service_Instantiates_Correctly()
    {
        Assert.That(_service, Is.Not.Null);
    }
}
