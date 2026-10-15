using System.Globalization;
using Microsoft.EntityFrameworkCore;
using InvenTrack.Data;

// Australian formats for dates (dd/MM/yyyy) and currency.
var culture = new CultureInfo("en-AU");
CultureInfo.DefaultThreadCurrentCulture = culture;
CultureInfo.DefaultThreadCurrentUICulture = culture;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddDbContext<InventoryContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("Inventory")));

var app = builder.Build();

// Create the database and add sample items on first run.
using (var scope = app.Services.CreateScope())
{
    SeedData.Initialise(scope.ServiceProvider.GetRequiredService<InventoryContext>());
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
