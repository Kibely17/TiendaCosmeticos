namespace TiendaCosmeticos.API.Models.Dtos
{
    public class MarcaDTO
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public string? Pais { get; set; }
    }
}
