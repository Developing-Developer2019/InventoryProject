namespace InventoryProject.Core.Model.API;

public class ItemUpdateRequest
{
    public required Guid Id { get; set; }
    public string? Reference { get; set; }
    public string? Name { get; set; }
    public decimal? Price { get; set; }
    public ICollection<Variation>? Variations { get; set; }
}
