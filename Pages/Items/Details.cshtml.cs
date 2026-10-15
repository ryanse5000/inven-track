using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using InvenTrack.Data;
using InvenTrack.Models;

namespace InvenTrack.Pages.Items;

public class DetailsModel(InventoryContext db) : PageModel
{
    public Item Item { get; private set; } = new();

    [BindProperty]
    public AdjustmentInput Adjustment { get; set; } = new();

    public class AdjustmentInput
    {
        /// <summary>"in" to receive stock, "out" to issue it.</summary>
        public string Direction { get; set; } = "in";

        [Range(1, 1_000_000, ErrorMessage = "Enter a quantity of at least 1.")]
        public int Amount { get; set; } = 1;

        [StringLength(200)]
        public string? Note { get; set; }
    }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        return await LoadAsync(id) ? Page() : NotFound();
    }

    public async Task<IActionResult> OnPostAdjustAsync(int id)
    {
        var item = await db.Items.FindAsync(id);
        if (item is null)
            return NotFound();

        var change = Adjustment.Direction == "out" ? -Adjustment.Amount : Adjustment.Amount;

        if (item.Quantity + change < 0)
            ModelState.AddModelError("Adjustment.Amount", $"Only {item.Quantity} in stock.");

        if (!ModelState.IsValid)
        {
            await LoadAsync(id);
            return Page();
        }

        item.Quantity += change;
        item.UpdatedAt = DateTime.Now;
        db.StockMovements.Add(new StockMovement
        {
            ItemId = id,
            Change = change,
            QuantityAfter = item.Quantity,
            Note = string.IsNullOrWhiteSpace(Adjustment.Note) ? (change > 0 ? "Stock received" : "Stock issued") : Adjustment.Note.Trim(),
        });
        await db.SaveChangesAsync();

        TempData["Message"] = change > 0
            ? $"Added {change} to stock. New balance: {item.Quantity}."
            : $"Removed {-change} from stock. New balance: {item.Quantity}.";
        return RedirectToPage(new { id });
    }

    private async Task<bool> LoadAsync(int id)
    {
        var item = await db.Items
            .Include(i => i.Movements.OrderByDescending(m => m.Timestamp).ThenByDescending(m => m.Id))
            .FirstOrDefaultAsync(i => i.Id == id);

        if (item is null)
            return false;

        Item = item;
        return true;
    }
}
