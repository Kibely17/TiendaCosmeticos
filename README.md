# Sistema de Gestion de Tienda de Cosmeticos - API

Tarea 3 de Programacion II: capa de Dominio e Infraestructura.

**Estudiante:** Kibely Altagracia Fulgencio Valdez

## Descripcion

API REST que administra una tienda de cosmeticos, separada en capas segun el
patron N-Capas.

| Entidad      | Descripcion                                              |
|--------------|----------------------------------------------------------|
| **Marca**    | Las marcas de cosmeticos que vende la tienda.            |
| **Producto** | Cada producto pertenece a una marca (uno a muchos).      |
| **Cliente**  | La persona que compra. Su cedula no se puede repetir.    |
| **Venta**    | Registra que un cliente compro un producto.              |

## Tecnologias

- ASP.NET Core 8 Web API
- Entity Framework Core 8 (enfoque Code First, con migraciones)
- SQL Server 2022
- Swagger / OpenAPI

## Arquitectura

```
TiendaCosmeticos.Domain/              Capa de dominio
├── Core/BaseEntity.cs                Id, Created y Updated comunes
└── Entities/                         Marca, Producto, Cliente, Venta

TiendaCosmeticos.Infrastructure/      Capa de acceso a datos
├── Context/ApplicationDbContext.cs
├── Migrations/
└── Repositories/
    ├── GenericRepository.cs          Operaciones comunes a cualquier entidad
    ├── MarcaRepository.cs            Consultas propias de Marca
    ├── ProductoRepository.cs         Consultas propias de Producto
    └── UnitOfWork.cs                 Agrupa los repositorios y guarda cambios

TiendaCosmeticos.API/                 Capa de presentacion
├── Controllers/                      Marcas, Productos, Clientes, Ventas
├── Models/Dtos/
└── Program.cs
```

Las referencias van en cadena: **API, Infrastructure y Domain**. El API no
referencia a Domain de forma directa porque ya le llega a traves de
Infrastructure.

### Por que un repositorio

Los controladores no acceden nunca al `ApplicationDbContext`. Toda interaccion
con una entidad pasa por su repositorio, de modo que si manana cambia la forma
de consultar los productos, se cambia en un solo lugar y no en cada controlador
que los necesite.

`Marca` y `Producto` tienen repositorio propio porque necesitan consultas que el
generico no puede resolver, como traer una marca con sus productos. `Cliente` y
`Venta` usan el repositorio generico, que cubre las operaciones comunes.

### Por que una unidad de trabajo

`GenericRepository` agrega, actualiza y elimina, pero **no guarda**. Quien guarda
es `UnitOfWork.Complete()`. Esto permite que varias operaciones entren juntas o
no entren: al registrar una venta hay que crear la venta y descontar el stock del
producto, y si lo segundo falla, lo primero tiene que deshacerse. Para eso la
unidad de trabajo expone `BeginTransaction`, `CommitTransaction` y
`RollbackTransaction`.

## Como ejecutar

### 1. Configurar la cadena de conexion

En `TiendaCosmeticos.API/appsettings.Development.json`, sustituir `TU_PASSWORD`
por la clave del usuario de SQL Server:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost,1433;Database=TiendaCosmeticos;User Id=sa;Password=TU_PASSWORD;MultipleActiveResultSets=true;TrustServerCertificate=true"
}
```

Con autenticacion de Windows la cadena seria:

```
Server=.;Database=TiendaCosmeticos;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=true
```

### 2. Crear la base de datos

La base se crea con la migracion. No hace falta ningun script:

```bash
dotnet ef database update --project TiendaCosmeticos.Infrastructure --startup-project TiendaCosmeticos.API
```

Las migraciones viven en el proyecto de Infraestructura, por eso se indica con
`--project`.

### 3. Levantar la API

```bash
dotnet run --project TiendaCosmeticos.API
```

Swagger queda disponible en `https://localhost:{puerto}/swagger`.

## Endpoints

Los cuatro controladores exponen el mismo CRUD:

| Metodo | Ruta                     | Respuesta       |
|--------|--------------------------|-----------------|
| GET    | /api/{recurso}           | 200 lista       |
| GET    | /api/{recurso}/{id}      | 200 o 404       |
| POST   | /api/{recurso}           | 201 + Location  |
| PUT    | /api/{recurso}/{id}      | 204, 400 o 404  |
| DELETE | /api/{recurso}/{id}      | 204, 400 o 404  |

Recursos disponibles: `marcas`, `productos`, `clientes`, `ventas`.

## Ejemplos de peticion

```json
POST /api/clientes
{
  "nombre": "Ana",
  "apellido": "Perez",
  "cedula": "001-1234567-8",
  "telefono": "809-555-0101",
  "correo": "ana@correo.com"
}
```

```json
POST /api/ventas
{
  "clienteId": 1,
  "productoId": 1,
  "cantidad": 3
}
```

Al registrar la venta se calcula el total con el precio del producto en ese
momento y se descuenta el stock.

## Validaciones

- El nombre de la marca, del producto y del cliente son obligatorios.
- La cedula del cliente es obligatoria y no se puede repetir.
- El precio del producto debe ser mayor que cero y el stock no puede ser negativo.
- No se puede crear un producto con una marca que no existe.
- No se puede eliminar una marca que tenga productos asociados.
- No se puede vender mas unidades de las que hay en stock.

## Nota sobre DTOs

La API nunca recibe ni devuelve entidades directamente. Toda entrada llega en un
DTO de creacion o actualizacion, y toda respuesta se construye mapeando la
entidad a su DTO de salida.
