using System.ComponentModel.DataAnnotations.Schema;

namespace TiendaCosmeticos.API.Entities
{
    /// <summary>
    /// Entidad Producto: representa un producto que vende la tienda.
    /// Cada producto pertenece a una Marca.
    /// </summary>
    public class Producto
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = string.Empty;

        // Indicamos el tipo exacto de la columna para no perder decimales.
        [Column(TypeName = "decimal(18,2)")]
        public decimal Precio { get; set; }

        public int Stock { get; set; }

        // Llave foranea hacia la tabla Marcas.
        public int MarcaId { get; set; }

        public DateTime Created { get; set; }

        public DateTime? Updated { get; set; }

        // Propiedad de navegacion opcional hacia la Marca a la que pertenece.
        public Marca? Marca { get; set; }
    }
}
