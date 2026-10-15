using InvenTrack.Models;

namespace InvenTrack.Data;

public static class SeedData
{
    public static void Initialise(InventoryContext db)
    {
        db.Database.EnsureCreated();

        if (db.Items.Any())
            return;

        var items = new List<Item>
        {
            new() { Name = "M8 hex bolt (box of 100)", Sku = "BLT-M8-100", Category = "Fasteners", Location = "Aisle 1, Bay A", Quantity = 42, ReorderLevel = 10, UnitPrice = 18.50m },
            new() { Name = "Cable ties 300mm (pack of 100)", Sku = "CTY-300", Category = "Electrical", Location = "Aisle 2, Bay C", Quantity = 6, ReorderLevel = 15, UnitPrice = 9.95m },
            new() { Name = "Safety glasses, clear", Sku = "PPE-GLS-CLR", Category = "PPE", Location = "Store room", Quantity = 25, ReorderLevel = 20, UnitPrice = 7.40m },
            new() { Name = "Hi-vis vest, size L", Sku = "PPE-VST-L", Category = "PPE", Location = "Store room", Quantity = 3, ReorderLevel = 5, UnitPrice = 14.00m },
            new() { Name = "2.5mm² twin & earth cable (100m)", Sku = "CBL-TPS-25", Category = "Electrical", Location = "Aisle 2, Bay A", Quantity = 8, ReorderLevel = 2, UnitPrice = 165.00m },
        };

        foreach (var item in items)
        {
            item.Movements.Add(new StockMovement { Change = item.Quantity, QuantityAfter = item.Quantity, Note = "Opening stock" });
        }

        db.Items.AddRange(items);
        db.SaveChanges();
    }
}
