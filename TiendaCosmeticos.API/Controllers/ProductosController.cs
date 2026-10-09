using Microsoft.AspNetCore.Mvc;
using TiendaCosmeticos.API.Data;
using TiendaCosmeticos.API.Entities;
using TiendaCosmeticos.API.Models.Dtos;

namespace TiendaCosmeticos.API.Controllers
{
    /// <summary>
    /// Controlador de Productos: expone el CRUD de la entidad Producto.
    /// Cada producto pertenece a una Marca, por eso validamos que la marca exista.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ProductosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET /api/productos
        [HttpGet]
        public ActionResult<List<ProductoDTO>> GetAll()
        {
            var productos = _context.Productos.ToList();

            var productosDTO = productos.Select(p => new ProductoDTO
            {
                Id = p.Id,
                Nombre = p.Nombre,
                Precio = p.Precio,
                Stock = p.Stock,
                MarcaId = p.MarcaId
            }).ToList();

            return Ok(productosDTO);
        }

        // GET /api/productos/{id}
        [HttpGet("{id}")]
        public ActionResult<ProductoDTO> GetById(int id)
        {
            var producto = _context.Productos.Find(id);
            if (producto == null)
            {
                return NotFound();
            }

            var productoDTO = new ProductoDTO
            {
                Id = producto.Id,
                Nombre = producto.Nombre,
                Precio = producto.Precio,
                Stock = producto.Stock,
                MarcaId = producto.MarcaId
            };

            return Ok(productoDTO);
        }

        // POST /api/productos
        [HttpPost]
        public ActionResult<ProductoDTO> Create([FromBody] CreateProductoDTO request)
        {
            if (string.IsNullOrWhiteSpace(request.Nombre))
            {
                return BadRequest("El nombre del producto es obligatorio.");
            }

            if (request.Precio <= 0)
            {
                return BadRequest("El precio debe ser mayor que cero.");
            }

            // Validamos que exista la marca antes de insertar el producto.
            var existeMarca = _context.Marcas.Any(m => m.Id == request.MarcaId);
            if (!existeMarca)
            {
                return BadRequest($"No existe una Marca con Id = {request.MarcaId}.");
            }

            var nuevoProducto = new Producto
            {
                Nombre = request.Nombre,
                Precio = request.Precio,
                Stock = request.Stock,
                MarcaId = request.MarcaId,
                Created = DateTime.Now
            };

            _context.Productos.Add(nuevoProducto);
            _context.SaveChanges();

            var productoDTO = new ProductoDTO
            {
                Id = nuevoProducto.Id,
                Nombre = nuevoProducto.Nombre,
                Precio = nuevoProducto.Precio,
                Stock = nuevoProducto.Stock,
                MarcaId = nuevoProducto.MarcaId
            };

            return CreatedAtAction(nameof(GetById), new { id = nuevoProducto.Id }, productoDTO);
        }

        // PUT /api/productos/{id}
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] UpdateProductoDTO request)
        {
            var producto = _context.Productos.Find(id);
            if (producto == null)
            {
                return NotFound();
            }

            if (string.IsNullOrWhiteSpace(request.Nombre))
            {
                return BadRequest("El nombre del producto es obligatorio.");
            }

            if (request.Precio <= 0)
            {
                return BadRequest("El precio debe ser mayor que cero.");
            }

            var existeMarca = _context.Marcas.Any(m => m.Id == request.MarcaId);
            if (!existeMarca)
            {
                return BadRequest($"No existe una Marca con Id = {request.MarcaId}.");
            }

            producto.Nombre = request.Nombre;
            producto.Precio = request.Precio;
            producto.Stock = request.Stock;
            producto.MarcaId = request.MarcaId;
            producto.Updated = DateTime.Now;

            _context.SaveChanges();

            return NoContent();
        }

        // DELETE /api/productos/{id}
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var producto = _context.Productos.Find(id);
            if (producto == null)
            {
                return NotFound();
            }

            _context.Productos.Remove(producto);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
