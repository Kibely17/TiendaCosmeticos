namespace TiendaCosmeticos.API.Models.Dtos
{
    /// <summary>
    /// DTO de entrada para crear una Marca.
    /// No incluye el Id porque lo genera la base de datos.
    /// </summary>
    public class CreateMarcaDTO
    {
        public string Nombre { get; set; } = string.Empty;

        public string? Pais { get; set; }
    }
}
