using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using InvenTrack.Data;
using InvenTrack.Models;

namespace InvenTrack.Pages;

public class IndexModel(InventoryContext db) : PageModel
{
    public int ItemCount { get; private set; }
    public int TotalUnits { get; private set; }
    public decimal TotalValue { get; private set; }
    public List<Item> LowStock { get; private set; } = [];
    public List<StockMovement> RecentMovements { get; private set; } = [];

    public async Task OnGetAsync()
    {
        var items = await db.Items.ToListAsync();

        ItemCount = items.Count;
        TotalUnits = items.Sum(i => i.Quantity);
        TotalValue = items.Sum(i => i.StockValue);
        LowStock = items.Where(i => i.IsLowStock).OrderBy(i => i.Quantity).ToList();

        RecentMovements = await db.StockMovements
            .Include(m => m.Item)
            .OrderByDescending(m => m.Timestamp)
            .ThenByDescending(m => m.Id)
            .Take(10)
            .ToListAsync();
    }
}
