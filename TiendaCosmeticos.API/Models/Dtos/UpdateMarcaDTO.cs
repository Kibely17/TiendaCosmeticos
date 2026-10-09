namespace TiendaCosmeticos.API.Models.Dtos
{
    /// <summary>
    /// DTO de entrada para actualizar una Marca.
    /// El Id no viaja en el cuerpo, viene en la ruta.
    /// </summary>
    public class UpdateMarcaDTO
    {
        public string Nombre { get; set; } = string.Empty;

        public string? Pais { get; set; }
    }
}
