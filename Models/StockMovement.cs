using System.ComponentModel.DataAnnotations;

namespace InvenTrack.Models;

public class StockMovement
{
    public int Id { get; set; }

    public int ItemId { get; set; }
    public Item? Item { get; set; }

    /// <summary>Positive for stock in, negative for stock out.</summary>
    public int Change { get; set; }

    [Display(Name = "Balance")]
    public int QuantityAfter { get; set; }

    [StringLength(200)]
    public string? Note { get; set; }

    public DateTime Timestamp { get; set; } = DateTime.Now;
}
