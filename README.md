# Tienda de Cosmeticos

Tarea 3 de Programacion II
Kibely Fulgencio

API en ASP.NET Core 8 con Entity Framework y SQL Server.
Entidades: Marca, Producto, Cliente y Venta.

Poner la clave de SQL Server en appsettings.Development.json donde dice TU_PASSWORD.

Crear la base:

    dotnet ef database update --project TiendaCosmeticos.Infrastructure --startup-project TiendaCosmeticos.API

Correr:

    dotnet run --project TiendaCosmeticos.API
