using TiendaCosmeticos.Domain.Core;

namespace TiendaCosmeticos.Domain.Entities
{
    public class Marca : BaseEntity
    {
        public string Nombre { get; set; } = string.Empty;

        public string? Pais { get; set; }

        public ICollection<Producto> Productos { get; set; } = new List<Producto>();
    }
}
