using System.ComponentModel.DataAnnotations;

namespace InvenTrack.Models;

public class Item
{
    public int Id { get; set; }

    [Required, StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required, StringLength(30)]
    [Display(Name = "SKU")]
    public string Sku { get; set; } = string.Empty;

    [StringLength(50)]
    public string? Category { get; set; }

    [StringLength(50)]
    public string? Location { get; set; }

    [Range(0, int.MaxValue)]
    public int Quantity { get; set; }

    [Range(0, int.MaxValue)]
    [Display(Name = "Reorder level")]
    public int ReorderLevel { get; set; }

    [Range(0, 1_000_000)]
    [DataType(DataType.Currency)]
    [Display(Name = "Unit price")]
    public decimal UnitPrice { get; set; }

    [Display(Name = "Last updated")]
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    public List<StockMovement> Movements { get; set; } = [];

    public bool IsLowStock => Quantity <= ReorderLevel;

    public decimal StockValue => Quantity * UnitPrice;
}
