using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using InvenTrack.Data;
using InvenTrack.Models;

namespace InvenTrack.Pages.Items;

public class DeleteModel(InventoryContext db) : PageModel
{
    public Item Item { get; private set; } = new();

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
        var item = await db.Items.FindAsync(id);
        if (item is not null)
        {
            db.Items.Remove(item);
            await db.SaveChangesAsync();
            TempData["Message"] = $"Deleted \"{item.Name}\".";
        }

        return RedirectToPage("Index");
    }
}
