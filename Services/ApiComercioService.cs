using System.Net.Http.Json;
using PapeleriaBlazor.Models;

namespace PapeleriaBlazor.Services;

/// <summary>
/// Servicio que consume la API del profesor.
/// Endpoints usados:
///   GET /api/comercio/productos
///   GET /api/comercio/productos/categoria/{id}
/// </summary>
public class ApiComercioService
{
    private readonly HttpClient _http;
    private readonly ILogger<ApiComercioService> _logger;

    public ApiComercioService(HttpClient http, ILogger<ApiComercioService> logger)
    {
        _http = http;
        _logger = logger;
    }

    // ── Endpoint 1: listado general ────────────────────────────────────────
    public async Task<List<ProductoApi>?> GetProductosAsync()
    {
        try
        {
            return await _http.GetFromJsonAsync<List<ProductoApi>>("/api/comercio/productos");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener productos de la API");
            return null; // La UI mostrará mensaje de error/carga fallida
        }
    }

    // ── Endpoint 2: filtro por categoría ──────────────────────────────────
    public async Task<List<ProductoApi>?> GetProductosPorCategoriaAsync(int categoriaId)
    {
        try
        {
            return await _http.GetFromJsonAsync<List<ProductoApi>>(
                $"/api/comercio/productos/categoria/{categoriaId}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener productos por categoría {Id}", categoriaId);
            return null;
        }
    }

    // ── Categorías (útil para el filtro de la pantalla de catálogo) ────────
    public async Task<List<CategoriaApi>?> GetCategoriasAsync()
    {
        try
        {
            return await _http.GetFromJsonAsync<List<CategoriaApi>>("/api/comercio/categorias");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener categorías de la API");
            return null;
        }
    }
}
