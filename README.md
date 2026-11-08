# Tienda de Cosmeticos

Tarea 3 de Programacion II.
Kibely Altagracia Fulgencio Valdez

API hecha en ASP.NET Core 8 con Entity Framework y SQL Server.
Las entidades son Marca, Producto, Cliente y Venta.

## Proyectos

- TiendaCosmeticos.Domain: las entidades
- TiendaCosmeticos.Infrastructure: el contexto, las migraciones y los repositorios
- TiendaCosmeticos.API: los controladores

## Para correrlo

Poner la clave de SQL Server en appsettings.Development.json donde dice TU_PASSWORD.

Crear la base:

    dotnet ef database update --project TiendaCosmeticos.Infrastructure --startup-project TiendaCosmeticos.API

Levantar el API:

    dotnet run --project TiendaCosmeticos.API

## Endpoints

Hay GET, POST, PUT y DELETE para marcas, productos, clientes y ventas.

    /api/marcas
    /api/productos
    /api/clientes
    /api/ventas

Cuando se registra una venta se calcula el total y se descuenta el stock del producto.
