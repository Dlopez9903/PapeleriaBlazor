using PapeleriaBlazor.Data;
using Microsoft.EntityFrameworkCore;

namespace PapeleriaBlazor.Services;

public class DashboardStats
{
    public int TotalClientes { get; set; }
    public int TotalPedidos { get; set; }
    public decimal TotalVentas { get; set; }
    public int PedidosPendientes { get; set; }
}

public class DashboardService
{
    private readonly PapeleriaDbContext _db;

    public DashboardService(PapeleriaDbContext db) => _db = db;

    public async Task<DashboardStats> GetStatsAsync()
    {
        return new DashboardStats
        {
            TotalClientes     = await _db.Clientes.CountAsync(),
            TotalPedidos      = await _db.Pedidos.CountAsync(),
            TotalVentas       = await _db.Pedidos
                                    .Where(p => p.Estado != "Cancelado")
                                    .SumAsync(p => (decimal?)p.Total) ?? 0,
            PedidosPendientes = await _db.Pedidos
                                    .Where(p => p.Estado == "Pendiente")
                                    .CountAsync()
        };
    }
}
