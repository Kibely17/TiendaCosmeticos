namespace TiendaCosmeticos.API.Models.Dtos
{
    /// <summary>
    /// DTO de entrada para crear un Producto.
    /// Contiene unicamente los datos que el cliente debe proporcionar.
    /// </summary>
    public class CreateProductoDTO
    {
        public string Nombre { get; set; } = string.Empty;

        public decimal Precio { get; set; }

        public int Stock { get; set; }

        public int MarcaId { get; set; }
    }
}
