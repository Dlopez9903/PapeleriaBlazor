# 📚 El Lápiz Feliz — Sistema de Gestión de Papelería

**Nombre:** López Jiménez Diego Abraham  
**Número de cuenta:** 20204971  
**Negocio asignado:** Papelería  
**Materia:** Programación para Web · 3a Parcial  
**Docente:** Ing. Ricardo Jaramillo Velasco  
**Fecha de entrega:** 29 de mayo de 2026  

---

##  API 

**Base URL:** `https://api-udec-pweb.azurewebsites.net`

| Endpoint | Uso |
|---|---|
| `GET /api/comercio/productos` | Listado general del catálogo |
| `GET /api/comercio/productos/categoria/{id}` | Filtro por categoría (Pantalla 2) |
| `GET /api/comercio/categorias` | Listado de categorías para el selector |

---

##  Estructura del proyecto

```
PapeleriaBlazor/
├── Models/
│   └── Models.cs            # Entidades BD + modelos JSON de la API
├── Data/
│   └── PapeleriaDbContext.cs # DbContext EF Core
├── Services/
│   ├── ApiComercioService.cs # Consumo de la API del profesor
│   ├── ClienteService.cs     # CRUD clientes
│   ├── PedidoService.cs      # Operaciones de pedidos
│   └── DashboardService.cs   # Stats para el dashboard
├── Pages/
│   ├── Index.razor           # Pantalla 1: Dashboard
│   ├── Catalogo.razor        # Pantalla 2: Catálogo (API)
│   ├── Clientes.razor        # Pantalla 3: Gestión clientes (CRUD)
│   └── Pedidos.razor         # Pantalla 4: Registro pedidos (integración)
├── Shared/
│   └── MainLayout.razor      # Layout con sidebar
├── wwwroot/css/
│   └── app.css               # Estilos (tema papelería editorial)
├── Program.cs
├── appsettings.json          # ← Aquí va tu connection string de Azure
└── README.md
```

---

##  Modelo de base de datos

```
Clientes (1) ──────< (N) Pedidos (1) ──────< (N) DetallesPedido
   Id                        Id                       Id
   Nombre                    ClienteId (FK)           PedidoId (FK)
   Telefono                  Fecha                    ProductoApiId 
   Email                     Total                    ProductoNombre
   FechaRegistro             Estado                   Cantidad
                                                      Precio
```

---

##  Pantallas del sistema

| # | Pantalla | Ruta | Descripción |
|---|---|---|---|
| 1 | Dashboard | `/` | KPIs de BD propia + conteo de productos de API |
| 2 | Catálogo | `/catalogo` | Lista de productos de la API con filtro por categoría |
| 3 | Clientes | `/clientes` | CRUD completo con confirmación de eliminación |
| 4 | Pedidos | `/pedidos` | Registro de pedidos integrando cliente (BD) + productos (API) |

---

##  Declaratoria de uso de IA

Herramienta utilizada: **Claude (Anthropic)** **Gemini**
