using Microsoft.AspNetCore.Mvc;
using TiendaCosmeticos.Application.Dtos;
using TiendaCosmeticos.Application.Services;

namespace TiendaCosmeticos.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MarcasController : ControllerBase
    {
        private readonly MarcaService _marcaService;

        public MarcasController(MarcaService marcaService)
        {
            _marcaService = marcaService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var response = _marcaService.GetAllMarcas();
            return response.Success ? Ok(response.Data) : BadRequest(response.Message);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var response = _marcaService.GetMarcaById(id);
            if (response.Success) return Ok(response.Data);
            return response.StatusCode == 404 ? NotFound(response.Message) : BadRequest(response.Message);
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateMarcaDTO request)
        {
            var response = _marcaService.CreateMarca(request);
            if (!response.Success) return BadRequest(response.Message);
            return CreatedAtAction(nameof(GetById), new { id = response.Data!.Id }, response.Data);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] UpdateMarcaDTO request)
        {
            var response = _marcaService.UpdateMarca(id, request);
            if (response.Success) return NoContent();
            return response.StatusCode == 404 ? NotFound(response.Message) : BadRequest(response.Message);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var response = _marcaService.DeleteMarca(id);
            if (response.Success) return NoContent();
            return response.StatusCode == 404 ? NotFound(response.Message) : BadRequest(response.Message);
        }
    }
}
