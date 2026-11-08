using Microsoft.AspNetCore.Mvc;
using TiendaCosmeticos.API.Models.Dtos;
using TiendaCosmeticos.Domain.Entities;
using TiendaCosmeticos.Infrastructure.Repositories;

namespace TiendaCosmeticos.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientesController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork;

        public ClientesController(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public ActionResult<List<ClienteDTO>> GetAll()
        {
            var clientes = _unitOfWork.Clientes.GetAll();
            return Ok(clientes.Select(Mapear).ToList());
        }

        [HttpGet("{id}")]
        public ActionResult<ClienteDTO> GetById(int id)
        {
            var cliente = _unitOfWork.Clientes.GetById(id);
            if (cliente == null) return NotFound();
            return Ok(Mapear(cliente));
        }

        [HttpPost]
        public ActionResult<ClienteDTO> Create([FromBody] CreateClienteDTO request)
        {
            if (string.IsNullOrWhiteSpace(request.Nombre))
                return BadRequest("El nombre del cliente es obligatorio.");
            if (string.IsNullOrWhiteSpace(request.Apellido))
                return BadRequest("El apellido del cliente es obligatorio.");
            if (string.IsNullOrWhiteSpace(request.Cedula))
                return BadRequest("La cedula del cliente es obligatoria.");

            var nuevo = new Cliente
            {
                Nombre = request.Nombre,
                Apellido = request.Apellido,
                Cedula = request.Cedula,
                Telefono = request.Telefono,
                Correo = request.Correo
            };

            _unitOfWork.Clientes.Create(nuevo);
            _unitOfWork.Complete();

            return CreatedAtAction(nameof(GetById), new { id = nuevo.Id }, Mapear(nuevo));
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] UpdateClienteDTO request)
        {
            var cliente = _unitOfWork.Clientes.GetById(id);
            if (cliente == null) return NotFound();

            if (string.IsNullOrWhiteSpace(request.Nombre))
                return BadRequest("El nombre del cliente es obligatorio.");
            if (string.IsNullOrWhiteSpace(request.Cedula))
                return BadRequest("La cedula del cliente es obligatoria.");

            cliente.Nombre = request.Nombre;
            cliente.Apellido = request.Apellido;
            cliente.Cedula = request.Cedula;
            cliente.Telefono = request.Telefono;
            cliente.Correo = request.Correo;

            _unitOfWork.Clientes.Update(cliente);
            _unitOfWork.Complete();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var cliente = _unitOfWork.Clientes.GetById(id);
            if (cliente == null) return NotFound();

            _unitOfWork.Clientes.Delete(cliente);
            _unitOfWork.Complete();

            return NoContent();
        }

        private static ClienteDTO Mapear(Cliente c) => new ClienteDTO
        {
            Id = c.Id,
            Nombre = c.Nombre,
            Apellido = c.Apellido,
            Cedula = c.Cedula,
            Telefono = c.Telefono,
            Correo = c.Correo
        };
    }
}
