using InventoryProject.Data.Data;
using InventoryProject.Service;
using InventoryProject.Service.Interface;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IItemService, ItemService>();

var connection = new SqliteConnection("Data Source=InventoryProject;Mode=Memory;Cache=Shared");
connection.Open();
builder.Services.AddDbContext<DataContext>(options => options.UseSqlite(connection));

var app = builder.Build();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
