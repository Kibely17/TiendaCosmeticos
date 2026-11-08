namespace TiendaCosmeticos.Application.Dtos
{
    public class UpdateProductoDTO
    {
        public string Nombre { get; set; } = string.Empty;

        public decimal Precio { get; set; }

        public int Stock { get; set; }

        public int MarcaId { get; set; }
    }
}
