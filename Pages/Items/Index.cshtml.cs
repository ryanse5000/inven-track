using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using InvenTrack.Data;
using InvenTrack.Models;

namespace InvenTrack.Pages.Items;

public class IndexModel(InventoryContext db) : PageModel
{
    public List<Item> Items { get; private set; } = [];
    public List<string> Categories { get; private set; } = [];

    [BindProperty(SupportsGet = true)]
    public string? Search { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? Category { get; set; }

    [BindProperty(SupportsGet = true)]
    public bool LowStockOnly { get; set; }

    public async Task OnGetAsync()
    {
        Categories = await db.Items
            .Where(i => i.Category != null)
            .Select(i => i.Category!)
            .Distinct()
            .OrderBy(c => c)
            .ToListAsync();

        var query = db.Items.AsQueryable();

        if (!string.IsNullOrWhiteSpace(Search))
        {
            var term = Search.Trim();
            query = query.Where(i => i.Name.Contains(term) || i.Sku.Contains(term));
        }

        if (!string.IsNullOrEmpty(Category))
            query = query.Where(i => i.Category == Category);

        if (LowStockOnly)
            query = query.Where(i => i.Quantity <= i.ReorderLevel);

        Items = await query.OrderBy(i => i.Name).ToListAsync();
    }
}
