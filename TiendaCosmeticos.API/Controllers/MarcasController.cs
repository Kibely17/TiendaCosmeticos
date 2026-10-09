using Microsoft.AspNetCore.Mvc;
using TiendaCosmeticos.API.Data;
using TiendaCosmeticos.API.Entities;
using TiendaCosmeticos.API.Models.Dtos;

namespace TiendaCosmeticos.API.Controllers
{
    /// <summary>
    /// Controlador de Marcas: expone el CRUD de la entidad Marca.
    /// Nunca recibe ni devuelve entidades, siempre trabaja contra DTOs.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class MarcasController : ControllerBase
    {
        // Contexto de EF Core inyectado por el contenedor de dependencias.
        private readonly ApplicationDbContext _context;

        public MarcasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET /api/marcas
        [HttpGet]
        public ActionResult<List<MarcaDTO>> GetAll()
        {
            var marcas = _context.Marcas.ToList();

            // Mapeo manual de entidades Marca a DTOs MarcaDTO.
            var marcasDTO = marcas.Select(m => new MarcaDTO
            {
                Id = m.Id,
                Nombre = m.Nombre,
                Pais = m.Pais
            }).ToList();

            return Ok(marcasDTO);
        }

        // GET /api/marcas/{id}
        [HttpGet("{id}")]
        public ActionResult<MarcaDTO> GetById(int id)
        {
            var marca = _context.Marcas.Find(id);
            if (marca == null)
            {
                return NotFound();
            }

            var marcaDTO = new MarcaDTO
            {
                Id = marca.Id,
                Nombre = marca.Nombre,
                Pais = marca.Pais
            };

            return Ok(marcaDTO);
        }

        // POST /api/marcas
        [HttpPost]
        public ActionResult<MarcaDTO> Create([FromBody] CreateMarcaDTO request)
        {
            if (string.IsNullOrWhiteSpace(request.Nombre))
            {
                return BadRequest("El nombre de la marca es obligatorio.");
            }

            // Mapear DTO de entrada a entidad.
            var nuevaMarca = new Marca
            {
                Nombre = request.Nombre,
                Pais = request.Pais,
                Created = DateTime.Now
            };

            _context.Marcas.Add(nuevaMarca);
            _context.SaveChanges();  // Al guardar, la base de datos genera el Id.

            // Mapeamos la entidad creada a un DTO de salida para la respuesta.
            var marcaDTO = new MarcaDTO
            {
                Id = nuevaMarca.Id,
                Nombre = nuevaMarca.Nombre,
                Pais = nuevaMarca.Pais
            };

            return CreatedAtAction(nameof(GetById), new { id = nuevaMarca.Id }, marcaDTO);
        }

        // PUT /api/marcas/{id}
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] UpdateMarcaDTO request)
        {
            var marca = _context.Marcas.Find(id);
            if (marca == null)
            {
                return NotFound();
            }

            if (string.IsNullOrWhiteSpace(request.Nombre))
            {
                return BadRequest("El nombre de la marca es obligatorio.");
            }

            // Actualizamos solo los campos editables, Created no se toca.
            marca.Nombre = request.Nombre;
            marca.Pais = request.Pais;
            marca.Updated = DateTime.Now;

            _context.SaveChanges();

            return NoContent();
        }

        // DELETE /api/marcas/{id}
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var marca = _context.Marcas.Find(id);
            if (marca == null)
            {
                return NotFound();
            }

            // No podemos borrar una marca que tenga productos asociados,
            // porque la llave foranea de Productos lo impide.
            var tieneProductos = _context.Productos.Any(p => p.MarcaId == id);
            if (tieneProductos)
            {
                return BadRequest("No se puede eliminar la marca porque tiene productos asociados.");
            }

            _context.Marcas.Remove(marca);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
