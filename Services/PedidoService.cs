using Microsoft.EntityFrameworkCore;
using PapeleriaBlazor.Data;
using PapeleriaBlazor.Models;

namespace PapeleriaBlazor.Services;

public class PedidoService
{
    private readonly PapeleriaDbContext _db;

    public PedidoService(PapeleriaDbContext db) => _db = db;

    // ── READ ───────────────────────────────────────────────────────────────
    public async Task<List<Pedido>> GetTodosAsync()
        => await _db.Pedidos
            .Include(p => p.Cliente)
            .Include(p => p.Detalles)
            .OrderByDescending(p => p.Fecha)
            .ToListAsync();

    public async Task<Pedido?> GetPorIdAsync(int id)
        => await _db.Pedidos
            .Include(p => p.Cliente)
            .Include(p => p.Detalles)
            .FirstOrDefaultAsync(p => p.Id == id);

    public async Task<int> ContarAsync()
        => await _db.Pedidos.CountAsync();

    public async Task<decimal> TotalVentasAsync()
        => await _db.Pedidos
            .Where(p => p.Estado != "Cancelado")
            .SumAsync(p => p.Total);

    // ── CREATE ─────────────────────────────────────────────────────────────
    public async Task<Pedido> CrearAsync(NuevoPedidoVm vm)
    {
        var pedido = new Pedido
        {
            ClienteId = vm.ClienteId,
            Fecha = DateTime.Now,
            Estado = vm.Estado,
            Total = vm.Total,
            Detalles = vm.Items.Select(i => new DetallePedido
            {
                ProductoApiId  = i.ProductoApiId,
                ProductoNombre = i.ProductoNombre,
                Cantidad       = i.Cantidad,
                Precio         = i.Precio
            }).ToList()
        };

        _db.Pedidos.Add(pedido);
        await _db.SaveChangesAsync();
        return pedido;
    }

    // ── UPDATE Estado ──────────────────────────────────────────────────────
    public async Task CambiarEstadoAsync(int pedidoId, string nuevoEstado)
    {
        var pedido = await _db.Pedidos.FindAsync(pedidoId)
            ?? throw new KeyNotFoundException($"Pedido {pedidoId} no encontrado.");
        pedido.Estado = nuevoEstado;
        await _db.SaveChangesAsync();
    }

    // ── DELETE ─────────────────────────────────────────────────────────────
    public async Task EliminarAsync(int id)
    {
        var pedido = await _db.Pedidos
            .Include(p => p.Detalles)
            .FirstOrDefaultAsync(p => p.Id == id)
            ?? throw new KeyNotFoundException($"Pedido {id} no encontrado.");

        _db.Pedidos.Remove(pedido);
        await _db.SaveChangesAsync();
    }
}
