using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PapeleriaBlazor.Models;

// ── ENTIDADES DE BASE DE DATOS PROPIA ─────────────────────────────────────

public class Cliente
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio")]
    [StringLength(100, ErrorMessage = "Máximo 100 caracteres")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "El teléfono es obligatorio")]
    [StringLength(15)]
    public string Telefono { get; set; } = string.Empty;

    [Required(ErrorMessage = "El email es obligatorio")]
    [EmailAddress(ErrorMessage = "Email inválido")]
    [StringLength(150)]
    public string Email { get; set; } = string.Empty;

    public DateTime FechaRegistro { get; set; } = DateTime.Now;

    // Navegación
    public ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
}

public class Pedido
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int ClienteId { get; set; }

    [ForeignKey(nameof(ClienteId))]
    public Cliente? Cliente { get; set; }

    public DateTime Fecha { get; set; } = DateTime.Now;

    [Column(TypeName = "decimal(10,2)")]
    public decimal Total { get; set; }

    [StringLength(50)]
    public string Estado { get; set; } = "Pendiente"; // Pendiente, Entregado, Cancelado

    // Navegación
    public ICollection<DetallePedido> Detalles { get; set; } = new List<DetallePedido>();
}

public class DetallePedido
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int PedidoId { get; set; }

    [ForeignKey(nameof(PedidoId))]
    public Pedido? Pedido { get; set; }

    /// <summary>
    /// Id del producto que viene de la API del profesor — NO se guarda el catálogo aquí
    /// </summary>
    public int ProductoApiId { get; set; }

    /// <summary>
    /// Se guarda el nombre para mostrar en historial aunque la API cambie
    /// </summary>
    [StringLength(150)]
    public string ProductoNombre { get; set; } = string.Empty;

    public int Cantidad { get; set; } = 1;

    [Column(TypeName = "decimal(10,2)")]
    public decimal Precio { get; set; }

    [NotMapped]
    public decimal Subtotal => Cantidad * Precio;
}

// ── MODELOS DE LA API DEL PROFESOR (deserialización JSON) ─────────────────

/// <summary>
/// Mapea la respuesta de GET /api/comercio/productos
/// Ajusta los nombres de propiedad si el JSON usa diferente casing
/// </summary>
public class ProductoApi
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public decimal Precio { get; set; }
    public int Stock { get; set; }
    public int CategoriaId { get; set; }
    public string? CategoriaNombre { get; set; }
    public string? ImagenUrl { get; set; }
}

/// <summary>
/// Mapea la respuesta de GET /api/comercio/categorias
/// </summary>
public class CategoriaApi
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
}

// ── VIEW MODELS ────────────────────────────────────────────────────────────

public class NuevoPedidoVm
{
    [Required(ErrorMessage = "Selecciona un cliente")]
    public int ClienteId { get; set; }

    public List<ItemCarrito> Items { get; set; } = new();

    [StringLength(50)]
    public string Estado { get; set; } = "Pendiente";

    public decimal Total => Items.Sum(i => i.Subtotal);
}

public class ItemCarrito
{
    public int ProductoApiId { get; set; }
    public string ProductoNombre { get; set; } = string.Empty;
    public decimal Precio { get; set; }
    public int Cantidad { get; set; } = 1;
    public decimal Subtotal => Precio * Cantidad;
}
