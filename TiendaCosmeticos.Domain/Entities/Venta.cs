using System.ComponentModel.DataAnnotations.Schema;
using TiendaCosmeticos.Domain.Core;

namespace TiendaCosmeticos.Domain.Entities
{
    public class Venta : BaseEntity
    {
        public int ClienteId { get; set; }

        public Cliente? Cliente { get; set; }

        public int ProductoId { get; set; }

        public Producto? Producto { get; set; }

        public int Cantidad { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal PrecioUnitario { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Total { get; set; }

        public DateTime Fecha { get; set; }
    }
}
