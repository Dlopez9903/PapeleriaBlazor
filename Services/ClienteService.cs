using Microsoft.EntityFrameworkCore;
using PapeleriaBlazor.Data;
using PapeleriaBlazor.Models;

namespace PapeleriaBlazor.Services;

public class ClienteService
{
    private readonly PapeleriaDbContext _db;

    public ClienteService(PapeleriaDbContext db) => _db = db;

    public async Task<List<Cliente>> GetTodosAsync()
        => await _db.Clientes.OrderByDescending(c => c.FechaRegistro).ToListAsync();

    public async Task<Cliente?> GetPorIdAsync(int id)
        => await _db.Clientes.FindAsync(id);

    public async Task<int> ContarAsync()
        => await _db.Clientes.CountAsync();

    public async Task AgregarAsync(Cliente cliente)
    {
        cliente.FechaRegistro = DateTime.Now;
        _db.Clientes.Add(cliente);
        await _db.SaveChangesAsync();
    }

    // ── UPDATE ─────────────────────────────────────────────────────────────
    public async Task ActualizarAsync(Cliente cliente)
    {
        _db.Clientes.Update(cliente);
        await _db.SaveChangesAsync();
    }

    // ── DELETE ─────────────────────────────────────────────────────────────
    /// <summary>
    /// Verifica si el cliente tiene pedidos antes de eliminar.
    /// Si tiene pedidos, lanza excepción con mensaje amigable.
    /// </summary>
    public async Task EliminarAsync(int id)
    {
        // Solo bloquea si tiene pedidos Pendientes — Entregados y Cancelados sí permiten eliminar
        var tienePedidosPendientes = await _db.Pedidos
            .AnyAsync(p => p.ClienteId == id && p.Estado == "Pendiente");

        if (tienePedidosPendientes)
            throw new InvalidOperationException(
                "No se puede eliminar el cliente porque tiene pedidos pendientes. " +
                "Primero marca todos sus pedidos como Entregados o Cancelados.");

        var cliente = await _db.Clientes.FindAsync(id)
            ?? throw new KeyNotFoundException($"Cliente {id} no encontrado.");

        _db.Clientes.Remove(cliente);
        await _db.SaveChangesAsync();
    }
}
