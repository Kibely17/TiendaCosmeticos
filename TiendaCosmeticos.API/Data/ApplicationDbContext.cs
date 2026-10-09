using Microsoft.EntityFrameworkCore;
using TiendaCosmeticos.API.Entities;

namespace TiendaCosmeticos.API.Data
{
    /// <summary>
    /// DbContext de la aplicacion: maneja la conexion a la base de datos y el mapeo de entidades.
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        // Constructor que recibe las opciones de configuracion (cadena de conexion, proveedor, etc.)
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSets: colecciones que representan las tablas de la base de datos.
        public DbSet<Marca> Marcas { get; set; } = null!;
        public DbSet<Producto> Productos { get; set; } = null!;
    }
}
