namespace TiendaCosmeticos.API.Models.Dtos
{
    /// <summary>
    /// DTO de salida para Marca: define exactamente que datos exponemos al cliente.
    /// </summary>
    public class MarcaDTO
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public string? Pais { get; set; }
    }
}
