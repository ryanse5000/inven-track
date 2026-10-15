# InvenTrack

A simple inventory tracking web app built with ASP.NET Core Razor Pages and SQLite. Track items, record stock coming in and going out, and see what needs reordering at a glance.

**Live demo:** <https://ryanse5000.github.io/InvenTrack/> (redirects to <https://inventrack-v3i5.onrender.com/>)

> The demo runs on Render's free plan. It sleeps when idle, so the first visit can take up to a minute, and its data resets to the sample items whenever it restarts or redeploys. Anyone with the link can change the data.

## Features

- **Dashboard** – total items, units in stock, stock value, low-stock count, a "needs reordering" list with progress bars and recent stock activity
- **Items list** – search by name or SKU, filter by category, show low-stock items only; colour-coded status (In stock, Low stock, Out of stock)
- **Add, edit and delete items** – name, SKU (unique), category, location, quantity, reorder level and unit price
- **Stock in / stock out** – adjust quantities with an optional note (e.g. PO or job number); stock can't go below zero
- **Stock history** – every change is logged with date, amount, running balance and note
- **Modern UI** – sidebar navigation, light and dark themes, works on phones
- **Australian formats** – dates as dd/MM/yyyy and prices in AUD

## Tech stack

| Area | Technology |
|---|---|
| Framework | ASP.NET Core 10 (Razor Pages) |
| Database | SQLite via Entity Framework Core 10 |
| UI | Bootstrap 5.3, custom theme, Lucide icons (inline SVG), Inter font |
| Hosting | Docker on Render (free plan) |
| Landing page | GitHub Pages redirect (`index.html`) |

## Getting started

**Requirements:** [.NET 10 SDK](https://dotnet.microsoft.com/download)

```bash
git clone https://github.com/ryanse5000/InvenTrack.git
cd InvenTrack
dotnet run
```

Then open <http://localhost:5139>.

On first run the app creates `inventory.db` in the project folder and adds five sample items. To start again from the sample data, stop the app and delete `inventory.db`.

## Project structure

```
InvenTrack/
├── Data/
│   ├── InventoryContext.cs   # EF Core database context
│   └── SeedData.cs           # Creates the database and sample items on startup
├── Helpers/
│   └── Icon.cs               # Inline SVG icon helper
├── Models/
│   ├── Item.cs               # An inventory item
│   └── StockMovement.cs      # A stock in/out entry
├── Pages/
│   ├── Index.cshtml          # Dashboard
│   ├── Items/                # List, details, create, edit and delete pages
│   └── Shared/               # Layout, sidebar navigation and shared partials
├── wwwroot/
│   ├── css/site.css          # Theme (colours are defined at the top)
│   └── js/site.js            # Theme toggle and auto-submitting filters
├── Program.cs                # App startup
├── Dockerfile                # Container build used by Render
└── index.html                # GitHub Pages redirect to the live demo (not part of the app)
```

## Configuration

The database location is set by the `Inventory` connection string in `appsettings.json`:

```json
"ConnectionStrings": {
  "Inventory": "Data Source=inventory.db"
}
```

Override it with the environment variable `ConnectionStrings__Inventory`, for example `Data Source=/app/data/inventory.db`. The app creates the folder if it doesn't exist.

## Deployment

### Render (current)

The app is deployed to [Render](https://render.com) as a Docker web service on the free plan:

1. **New + → Web Service**, connect the GitHub repo
2. Language **Docker**, branch **main**, instance type **Free**
3. Deploy

Every push to `main` redeploys automatically. The [Dockerfile](Dockerfile) runs the app on port 10000 as a non-root user, with the database in `/app/data`.

### GitHub Pages

GitHub Pages is enabled for this repo and serves [index.html](index.html), which redirects visitors to the Render app. It doesn't affect the app build.

## Notes

- **No login** – this is a demo, so there's no authentication. Add some before using it with real data.
- **Database changes** – the schema is created with `EnsureCreated()` rather than migrations. If you change the models, delete `inventory.db` so it can be recreated.
- **SQLite journal mode** – the app uses SQLite's classic journal (not WAL) so it also works on network storage, such as Azure App Service.
