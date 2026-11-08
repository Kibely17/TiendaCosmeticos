using Microsoft.AspNetCore.Mvc;
using TiendaCosmeticos.API.Models.Dtos;
using TiendaCosmeticos.Domain.Entities;
using TiendaCosmeticos.Infrastructure.Repositories;

namespace TiendaCosmeticos.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VentasController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork;

        public VentasController(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public ActionResult<List<VentaDTO>> GetAll()
        {
            var ventas = _unitOfWork.Ventas.GetAll();
            return Ok(ventas.Select(Mapear).ToList());
        }

        [HttpGet("{id}")]
        public ActionResult<VentaDTO> GetById(int id)
        {
            var venta = _unitOfWork.Ventas.GetById(id);
            if (venta == null) return NotFound();
            return Ok(Mapear(venta));
        }

        [HttpPost]
        public ActionResult<VentaDTO> Create([FromBody] CreateVentaDTO request)
        {
            if (request.Cantidad <= 0)
                return BadRequest("La cantidad debe ser mayor que cero.");

            var cliente = _unitOfWork.Clientes.GetById(request.ClienteId);
            if (cliente == null) return BadRequest("El cliente indicado no existe.");

            var producto = _unitOfWork.Productos.GetById(request.ProductoId);
            if (producto == null) return BadRequest("El producto indicado no existe.");

            if (producto.Stock < request.Cantidad)
                return BadRequest($"Stock insuficiente. Disponible: {producto.Stock}.");

            var venta = new Venta
            {
                ClienteId = request.ClienteId,
                ProductoId = request.ProductoId,
                Cantidad = request.Cantidad,
                PrecioUnitario = producto.Precio,
                Total = producto.Precio * request.Cantidad,
                Fecha = DateTime.Now
            };

            producto.Stock -= request.Cantidad;

            _unitOfWork.BeginTransaction();
            try
            {
                _unitOfWork.Ventas.Create(venta);
                _unitOfWork.Productos.Update(producto);
                _unitOfWork.Complete();
                _unitOfWork.CommitTransaction();
            }
            catch
            {
                _unitOfWork.RollbackTransaction();
                throw;
            }

            return CreatedAtAction(nameof(GetById), new { id = venta.Id }, Mapear(venta));
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] UpdateVentaDTO request)
        {
            var venta = _unitOfWork.Ventas.GetById(id);
            if (venta == null) return NotFound();

            if (request.Cantidad <= 0)
                return BadRequest("La cantidad debe ser mayor que cero.");

            venta.Cantidad = request.Cantidad;
            venta.Total = venta.PrecioUnitario * request.Cantidad;

            _unitOfWork.Ventas.Update(venta);
            _unitOfWork.Complete();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var venta = _unitOfWork.Ventas.GetById(id);
            if (venta == null) return NotFound();

            _unitOfWork.Ventas.Delete(venta);
            _unitOfWork.Complete();

            return NoContent();
        }

        private static VentaDTO Mapear(Venta v) => new VentaDTO
        {
            Id = v.Id,
            ClienteId = v.ClienteId,
            ProductoId = v.ProductoId,
            Cantidad = v.Cantidad,
            PrecioUnitario = v.PrecioUnitario,
            Total = v.Total,
            Fecha = v.Fecha
        };
    }
}
