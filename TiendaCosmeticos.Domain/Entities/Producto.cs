using System.ComponentModel.DataAnnotations.Schema;
using TiendaCosmeticos.Domain.Core;

namespace TiendaCosmeticos.Domain.Entities
{
    public class Producto : BaseEntity
    {
        public string Nombre { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Precio { get; set; }

        public int Stock { get; set; }

        public int MarcaId { get; set; }

        public Marca? Marca { get; set; }

        public ICollection<Venta> Ventas { get; set; } = new List<Venta>();
    }
}
