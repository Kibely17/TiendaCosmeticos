using TiendaCosmeticos.Domain.Core;

namespace TiendaCosmeticos.Domain.Entities
{
    public class Cliente : BaseEntity
    {
        public string Nombre { get; set; } = string.Empty;

        public string Apellido { get; set; } = string.Empty;

        public string Cedula { get; set; } = string.Empty;

        public string? Telefono { get; set; }

        public string? Correo { get; set; }

        public ICollection<Venta> Ventas { get; set; } = new List<Venta>();
    }
}
