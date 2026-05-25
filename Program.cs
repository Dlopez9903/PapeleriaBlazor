using Microsoft.EntityFrameworkCore;
using PapeleriaBlazor.Data;
using PapeleriaBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// ── Blazor Server ──────────────────────────────────────────────────────────
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// ── Entity Framework Core → Azure SQL ─────────────────────────────────────
builder.Services.AddDbContext<PapeleriaDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ── HttpClient para la API del profesor ───────────────────────────────────
builder.Services.AddHttpClient<ApiComercioService>(client =>
{
    client.BaseAddress = new Uri(
        builder.Configuration["ApiConfig:BaseUrl"] ?? "https://api-udec-pweb.azurewebsites.net");
});

// ── Servicios de negocio ───────────────────────────────────────────────────
builder.Services.AddScoped<ClienteService>();
builder.Services.AddScoped<PedidoService>();
builder.Services.AddScoped<DashboardService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
