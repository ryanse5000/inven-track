using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using InvenTrack.Data;
using InvenTrack.Models;

namespace InvenTrack.Pages.Items;

public class CreateModel(InventoryContext db) : PageModel
{
    [BindProperty]
    public Item Item { get; set; } = new();

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        Item.Sku = Item.Sku.Trim().ToUpperInvariant();

        if (await db.Items.AnyAsync(i => i.Sku == Item.Sku))
            ModelState.AddModelError("Item.Sku", "An item with this SKU already exists.");

        if (!ModelState.IsValid)
            return Page();

        Item.UpdatedAt = DateTime.Now;
        if (Item.Quantity > 0)
            Item.Movements.Add(new StockMovement { Change = Item.Quantity, QuantityAfter = Item.Quantity, Note = "Opening stock" });

        db.Items.Add(Item);
        await db.SaveChangesAsync();

        TempData["Message"] = $"Added \"{Item.Name}\".";
        return RedirectToPage("Details", new { id = Item.Id });
    }
}
