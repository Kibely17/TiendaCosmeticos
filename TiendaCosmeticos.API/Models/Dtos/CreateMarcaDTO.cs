namespace TiendaCosmeticos.API.Models.Dtos
{
    public class CreateMarcaDTO
    {
        public string Nombre { get; set; } = string.Empty;

        public string? Pais { get; set; }
    }
}
