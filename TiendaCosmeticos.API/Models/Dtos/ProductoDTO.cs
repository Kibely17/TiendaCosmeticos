namespace TiendaCosmeticos.API.Models.Dtos
{
    /// <summary>
    /// DTO de salida para Producto: controlamos que datos del producto exponemos.
    /// Devolvemos el MarcaId y no la entidad Marca completa, para no arrastrar
    /// la navegacion ni generar referencias circulares en el JSON.
    /// </summary>
    public class ProductoDTO
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public decimal Precio { get; set; }

        public int Stock { get; set; }

        public int MarcaId { get; set; }
    }
}
