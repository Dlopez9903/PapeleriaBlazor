using Microsoft.EntityFrameworkCore;
using PapeleriaBlazor.Models;

namespace PapeleriaBlazor.Data;

public class PapeleriaDbContext : DbContext
{
    public PapeleriaDbContext(DbContextOptions<PapeleriaDbContext> options)
        : base(options) { }

    public DbSet<Cliente> Clientes => Set<Cliente>();
    public DbSet<Pedido> Pedidos => Set<Pedido>();
    public DbSet<DetallePedido> DetallesPedido => Set<DetallePedido>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Cliente>(e =>
        {
            e.ToTable("Clientes");
            e.HasKey(c => c.Id);
            e.Property(c => c.Nombre).IsRequired().HasMaxLength(100);
            e.Property(c => c.Email).IsRequired().HasMaxLength(150);
            e.Property(c => c.Telefono).IsRequired().HasMaxLength(15);
        });

        modelBuilder.Entity<Pedido>(e =>
        {
            e.ToTable("Pedidos");
            e.HasKey(p => p.Id);
            e.Property(p => p.Total).HasColumnType("decimal(10,2)");
            e.Property(p => p.Estado).HasMaxLength(50).HasDefaultValue("Pendiente");

            e.HasOne(p => p.Cliente)
             .WithMany(c => c.Pedidos)
             .HasForeignKey(p => p.ClienteId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        // ── DetallePedido ──────────────────────────────────────────────────
        modelBuilder.Entity<DetallePedido>(e =>
        {
            e.ToTable("DetallesPedido");
            e.HasKey(d => d.Id);
            e.Property(d => d.Precio).HasColumnType("decimal(10,2)");
            e.Property(d => d.ProductoNombre).HasMaxLength(150);

            e.HasOne(d => d.Pedido)
             .WithMany(p => p.Detalles)
             .HasForeignKey(d => d.PedidoId)
             .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
