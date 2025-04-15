using InventoryProject.API.Controller;
using InventoryProject.Core.Model;
using Microsoft.AspNetCore.Mvc;

namespace InventoryProject.Tests;

[TestFixture]
public class InventoryControllerTests
{
    private InventoryController _controller;

    [SetUp]
    public void Setup()
    {
        _controller = new InventoryController();
    }

    [Test]
    public async Task GetAllItems_ReturnsOk()
    {
        var result = await _controller.GetAllItems();
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
    }

    [Test]
    public async Task GetItemById_ReturnsOk()
    {
        var result = await _controller.GetItemById(Guid.NewGuid());
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
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