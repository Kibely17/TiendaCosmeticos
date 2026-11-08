using Microsoft.AspNetCore.Mvc;
using TiendaCosmeticos.API.Models.Dtos;
using TiendaCosmeticos.Domain.Entities;
using TiendaCosmeticos.Infrastructure.Repositories;

namespace TiendaCosmeticos.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductosController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork;

        public ProductosController(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public ActionResult<List<ProductoDTO>> GetAll()
        {
            var productos = _unitOfWork.Productos.GetAllProductosWithMarca();
            return Ok(productos.Select(Mapear).ToList());
        }

        [HttpGet("{id}")]
        public ActionResult<ProductoDTO> GetById(int id)
        {
            var producto = _unitOfWork.Productos.GetById(id);
            if (producto == null) return NotFound();
            return Ok(Mapear(producto));
        }

        [HttpPost]
        public ActionResult<ProductoDTO> Create([FromBody] CreateProductoDTO request)
        {
            if (string.IsNullOrWhiteSpace(request.Nombre))
                return BadRequest("El nombre del producto es obligatorio.");
            if (request.Precio <= 0)
                return BadRequest("El precio debe ser mayor que cero.");
            if (request.Stock < 0)
                return BadRequest("El stock no puede ser negativo.");
            if (_unitOfWork.Marcas.GetById(request.MarcaId) == null)
                return BadRequest("La marca indicada no existe.");

            var nuevo = new Producto
            {
                Nombre = request.Nombre,
                Precio = request.Precio,
                Stock = request.Stock,
                MarcaId = request.MarcaId
            };

            _unitOfWork.Productos.Create(nuevo);
            _unitOfWork.Complete();

            return CreatedAtAction(nameof(GetById), new { id = nuevo.Id }, Mapear(nuevo));
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] UpdateProductoDTO request)
        {
            var producto = _unitOfWork.Productos.GetById(id);
            if (producto == null) return NotFound();

            if (string.IsNullOrWhiteSpace(request.Nombre))
                return BadRequest("El nombre del producto es obligatorio.");
            if (request.Precio <= 0)
                return BadRequest("El precio debe ser mayor que cero.");
            if (_unitOfWork.Marcas.GetById(request.MarcaId) == null)
                return BadRequest("La marca indicada no existe.");

            producto.Nombre = request.Nombre;
            producto.Precio = request.Precio;
            producto.Stock = request.Stock;
            producto.MarcaId = request.MarcaId;

            _unitOfWork.Productos.Update(producto);
            _unitOfWork.Complete();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var producto = _unitOfWork.Productos.GetById(id);
            if (producto == null) return NotFound();

            _unitOfWork.Productos.Delete(producto);
            _unitOfWork.Complete();

            return NoContent();
        }

        private static ProductoDTO Mapear(Producto p) => new ProductoDTO
        {
            Id = p.Id,
            Nombre = p.Nombre,
            Precio = p.Precio,
            Stock = p.Stock,
            MarcaId = p.MarcaId
        };
    }
}
