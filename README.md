# Sistema de Gestion de Tienda de Cosmeticos - API

Tarea 2 de Programacion II: CRUD API en ASP.NET Core conectado a SQL Server.

**Estudiante:** Kibely Altagracia Fulgencio Valdez

## Descripcion

API REST que administra las dos entidades principales de una tienda de cosmeticos:

- **Marca**: las marcas de cosmeticos que vende la tienda.
- **Producto**: los productos disponibles. Cada producto pertenece a una marca (relacion uno a muchos).

## Tecnologias

- ASP.NET Core 8 Web API (con Controllers)
- Entity Framework Core 8 (enfoque Database First)
- SQL Server 2022
- Swagger / OpenAPI

## Estructura del proyecto

```
TiendaCosmeticos.API/
├── Controllers/     MarcasController.cs, ProductosController.cs
├── Data/            ApplicationDbContext.cs
├── Entities/        Marca.cs, Producto.cs
├── Models/Dtos/     DTOs de entrada y de salida
├── Program.cs
└── appsettings.Development.json
```

## Como ejecutar

### 1. Crear la base de datos

Ejecutar el script `Database.sql` en SQL Server. Crea la base `TiendaCosmeticos`,
las tablas `Marcas` y `Productos`, y carga datos de ejemplo.

### 2. Configurar la cadena de conexion

En `TiendaCosmeticos.API/appsettings.Development.json`, sustituir `TU_PASSWORD`
por la clave del usuario de SQL Server:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost,1433;Database=TiendaCosmeticos;User Id=sa;Password=TU_PASSWORD;MultipleActiveResultSets=true;TrustServerCertificate=true"
}
```

Si se usa autenticacion de Windows, la cadena seria:

```
Server=.;Database=TiendaCosmeticos;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=true
```

### 3. Levantar la API

```bash
cd TiendaCosmeticos.API
dotnet run
```

Swagger queda disponible en `https://localhost:{puerto}/swagger`.

## Endpoints

### Marcas

| Metodo | Ruta               | Respuesta        |
|--------|--------------------|------------------|
| GET    | /api/marcas        | 200 lista        |
| GET    | /api/marcas/{id}   | 200 o 404        |
| POST   | /api/marcas        | 201 + Location   |
| PUT    | /api/marcas/{id}   | 204 o 404        |
| DELETE | /api/marcas/{id}   | 204, 404 o 400   |

### Productos

| Metodo | Ruta                  | Respuesta        |
|--------|-----------------------|------------------|
| GET    | /api/productos        | 200 lista        |
| GET    | /api/productos/{id}   | 200 o 404        |
| POST   | /api/productos        | 201 + Location   |
| PUT    | /api/productos/{id}   | 204 o 404        |
| DELETE | /api/productos/{id}   | 204 o 404        |

## Ejemplos de peticion

Crear una marca:

```json
POST /api/marcas
{
  "nombre": "Revlon",
  "pais": "Estados Unidos"
}
```

Crear un producto:

```json
POST /api/productos
{
  "nombre": "Delineador Negro",
  "precio": 320.50,
  "stock": 40,
  "marcaId": 3
}
```

## Validaciones

- El nombre de la marca y del producto son obligatorios.
- El precio del producto debe ser mayor que cero.
- No se puede crear un producto con una marca que no existe.
- No se puede eliminar una marca que tenga productos asociados.

## Nota sobre DTOs

La API nunca recibe ni devuelve entidades directamente. Toda entrada llega en un
DTO de creacion o actualizacion, y toda respuesta se construye mapeando
manualmente la entidad a su DTO de salida.
