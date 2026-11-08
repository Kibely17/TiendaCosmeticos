namespace TiendaCosmeticos.API.Models.Dtos
{
    public class CreateVentaDTO
    {
        public int ClienteId { get; set; }
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }
    }
}
