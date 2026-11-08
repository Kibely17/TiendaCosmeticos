# Tienda de Cosmeticos - API

Tarea 4 de Programacion II.

**Estudiante:** Kibely Altagracia Fulgencio Valdez

## Descripcion

API REST para una tienda de cosmeticos, dividida en capas. Maneja cuatro
entidades: Marca, Producto, Cliente y Venta.

## Tecnologias

- ASP.NET Core 8 Web API
- Entity Framework Core 8 (Code First)
- SQL Server 2022
- Swagger

## Estructura

```
TiendaCosmeticos.Domain/
  Core/BaseEntity.cs
  Entities/            Marca, Producto, Cliente, Venta

TiendaCosmeticos.Infrastructure/
  Context/             ApplicationDbContext
  Migrations/
  Repositories/        GenericRepository, MarcaRepository,
                       ProductoRepository, UnitOfWork

TiendaCosmeticos.Application/
  Core/ApiResponse.cs
  Dtos/
  Services/            MarcaService, ProductoService,
                       ClienteService, VentaService

TiendaCosmeticos.API/
  Controllers/         Marcas, Productos, Clientes, Ventas
```

Cada proyecto referencia solo al siguiente: API, Application, Infrastructure,
Domain.

Las validaciones estan en los servicios, no en los controladores. Todos los
servicios devuelven un `ApiResponse<T>` con las propiedades `Success`,
`Message`, `Data` y `StatusCode`, y el controlador lo traduce a la respuesta
HTTP que corresponda.

## Como ejecutar

1. En `TiendaCosmeticos.API/appsettings.Development.json` sustituir
   `TU_PASSWORD` por la clave de SQL Server.

2. Crear la base de datos:

```bash
dotnet ef database update --project TiendaCosmeticos.Infrastructure --startup-project TiendaCosmeticos.API
```

3. Levantar el API:

```bash
dotnet run --project TiendaCosmeticos.API
```

Swagger queda en `/swagger`.

## Endpoints

Los cuatro recursos (`marcas`, `productos`, `clientes`, `ventas`) tienen el
mismo CRUD:

```
GET    /api/{recurso}
GET    /api/{recurso}/{id}
POST   /api/{recurso}
PUT    /api/{recurso}/{id}
DELETE /api/{recurso}/{id}
```

Ejemplo para registrar una venta:

```json
POST /api/ventas
{
  "clienteId": 1,
  "productoId": 1,
  "cantidad": 3
}
```

El total se calcula con el precio que tenga el producto en ese momento y se
descuenta el stock. Las dos cosas se guardan en una transaccion.

## Validaciones

- El nombre de la marca, del producto y del cliente son obligatorios.
- La cedula del cliente es obligatoria y no se repite.
- El correo debe tener formato valido.
- El precio debe ser mayor que cero y el stock no puede ser negativo.
- No se puede crear un producto con una marca que no existe.
- No se puede eliminar una marca que tenga productos.
- No se puede eliminar un cliente que tenga ventas.
- No se puede vender mas unidades de las que hay en stock.
