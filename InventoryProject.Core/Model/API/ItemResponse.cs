using InventoryProject.Core.Constants;
using InventoryProject.Core.Enum;

namespace InventoryProject.Core.Model.API;

public record ItemResponse
{
    public Guid Id { get; set; }
    public string Reference { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public ICollection<Variation> Variations { get; set; } = new List<Variation>();
    
    // Returning extra values based on image UI.png
    public int StockCount => Variations.Sum(variation => variation.Quantity);
    public string Status => StockCount > 0 ? StringConstants.InStock : StringConstants.SoldOut;
    public DiscountAmount? DiscountAmount { get; set; }
}