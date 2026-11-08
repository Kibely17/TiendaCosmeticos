using Microsoft.AspNetCore.Mvc;
using TiendaCosmeticos.API.Models.Dtos;
using TiendaCosmeticos.Domain.Entities;
using TiendaCosmeticos.Infrastructure.Repositories;

namespace TiendaCosmeticos.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MarcasController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork;

        public MarcasController(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public ActionResult<List<MarcaDTO>> GetAll()
        {
            var marcas = _unitOfWork.Marcas.GetAll();
            return Ok(marcas.Select(Mapear).ToList());
        }

        [HttpGet("{id}")]
        public ActionResult<MarcaDTO> GetById(int id)
        {
            var marca = _unitOfWork.Marcas.GetById(id);
            if (marca == null) return NotFound();
            return Ok(Mapear(marca));
        }

        [HttpPost]
        public ActionResult<MarcaDTO> Create([FromBody] CreateMarcaDTO request)
        {
            if (string.IsNullOrWhiteSpace(request.Nombre))
                return BadRequest("El nombre de la marca es obligatorio.");

            var nueva = new Marca { Nombre = request.Nombre, Pais = request.Pais };

            _unitOfWork.Marcas.Create(nueva);
            _unitOfWork.Complete();

            return CreatedAtAction(nameof(GetById), new { id = nueva.Id }, Mapear(nueva));
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] UpdateMarcaDTO request)
        {
            var marca = _unitOfWork.Marcas.GetById(id);
            if (marca == null) return NotFound();

            if (string.IsNullOrWhiteSpace(request.Nombre))
                return BadRequest("El nombre de la marca es obligatorio.");

            marca.Nombre = request.Nombre;
            marca.Pais = request.Pais;

            _unitOfWork.Marcas.Update(marca);
            _unitOfWork.Complete();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var marca = _unitOfWork.Marcas.GetById(id);
            if (marca == null) return NotFound();

            if (_unitOfWork.Marcas.TieneProductos(id))
                return BadRequest("No se puede eliminar la marca porque tiene productos asociados.");

            _unitOfWork.Marcas.Delete(marca);
            _unitOfWork.Complete();

            return NoContent();
        }

        private static MarcaDTO Mapear(Marca m) => new MarcaDTO
        {
            Id = m.Id,
            Nombre = m.Nombre,
            Pais = m.Pais
        };
    }
}
