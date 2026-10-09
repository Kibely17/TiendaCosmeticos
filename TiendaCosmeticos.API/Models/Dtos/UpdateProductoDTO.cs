namespace TiendaCosmeticos.API.Models.Dtos
{
    /// <summary>
    /// DTO de entrada para actualizar un Producto.
    /// El Id no viaja en el cuerpo, viene en la ruta.
    /// </summary>
    public class UpdateProductoDTO
    {
        public string Nombre { get; set; } = string.Empty;

        public decimal Precio { get; set; }

        public int Stock { get; set; }

        public int MarcaId { get; set; }
    }
}
