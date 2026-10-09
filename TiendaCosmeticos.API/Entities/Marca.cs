namespace TiendaCosmeticos.API.Entities
{
    /// <summary>
    /// Entidad Marca: representa una marca de cosmeticos.
    /// Cada propiedad corresponde a una columna de la tabla Marcas.
    /// </summary>
    public class Marca
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public string? Pais { get; set; }

        public DateTime Created { get; set; }

        public DateTime? Updated { get; set; }

        // Propiedad de navegacion: una Marca tiene muchos Productos (uno a muchos).
        public ICollection<Producto> Productos { get; set; } = new List<Producto>();
    }
}
