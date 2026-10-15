using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using InvenTrack.Data;
using InvenTrack.Models;

namespace InvenTrack.Pages.Items;

public class EditModel(InventoryContext db) : PageModel
{
    [BindProperty]
    public Item Item { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var item = await db.Items.FindAsync(id);
        if (item is null)
            return NotFound();

        Item = item;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        var existing = await db.Items.FindAsync(id);
        if (existing is null)
            return NotFound();

        Item.Id = id;
        Item.Sku = Item.Sku.Trim().ToUpperInvariant();

        if (await db.Items.AnyAsync(i => i.Sku == Item.Sku && i.Id != id))
            ModelState.AddModelError("Item.Sku", "Another item already uses this SKU.");

        if (!ModelState.IsValid)
            return Page();

        // Record any quantity change so the history stays complete.
        var change = Item.Quantity - existing.Quantity;
        if (change != 0)
            db.StockMovements.Add(new StockMovement { ItemId = id, Change = change, QuantityAfter = Item.Quantity, Note = "Quantity edited" });

        existing.Name = Item.Name;
        existing.Sku = Item.Sku;
        existing.Category = Item.Category;
        existing.Location = Item.Location;
        existing.Quantity = Item.Quantity;
        existing.ReorderLevel = Item.ReorderLevel;
        existing.UnitPrice = Item.UnitPrice;
        existing.UpdatedAt = DateTime.Now;

        await db.SaveChangesAsync();

        TempData["Message"] = $"Saved changes to \"{existing.Name}\".";
        return RedirectToPage("Details", new { id });
    }
}
