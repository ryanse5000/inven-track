using Microsoft.EntityFrameworkCore;
using InvenTrack.Models;

namespace InvenTrack.Data;

public class InventoryContext(DbContextOptions<InventoryContext> options) : DbContext(options)
{
    public DbSet<Item> Items => Set<Item>();
    public DbSet<StockMovement> StockMovements => Set<StockMovement>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Item>().HasIndex(i => i.Sku).IsUnique();

        // SQLite has no native decimal type; store as double so it can be sorted and summed.
        modelBuilder.Entity<Item>().Property(i => i.UnitPrice).HasConversion<double>();

        modelBuilder.Entity<StockMovement>()
            .HasOne(m => m.Item)
            .WithMany(i => i.Movements)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
