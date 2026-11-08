using TiendaCosmeticos.Application.Core;
using TiendaCosmeticos.Application.Dtos;
using TiendaCosmeticos.Domain.Entities;
using TiendaCosmeticos.Infrastructure.Repositories;

namespace TiendaCosmeticos.Application.Services
{
    public class ClienteService
    {
        private readonly UnitOfWork _unitOfWork;

        public ClienteService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public ApiResponse<IEnumerable<ClienteDTO>> GetAllClientes()
        {
            var clientes = _unitOfWork.Clientes.GetAll().Select(Mapear);
            return ApiResponse<IEnumerable<ClienteDTO>>.SuccessResponse(clientes);
        }

        public ApiResponse<ClienteDTO> GetClienteById(int id)
        {
            var cliente = _unitOfWork.Clientes.GetById(id);
            if (cliente == null)
                return ApiResponse<ClienteDTO>.ErrorResponse("Cliente no encontrado.", 404);

            return ApiResponse<ClienteDTO>.SuccessResponse(Mapear(cliente));
        }

        public ApiResponse<ClienteDTO> CreateCliente(CreateClienteDTO request)
        {
            var error = Validar(request.Nombre, request.Apellido, request.Cedula, request.Correo);
            if (error != null)
                return ApiResponse<ClienteDTO>.ErrorResponse(error);

            if (_unitOfWork.Clientes.GetAll().Any(c => c.Cedula == request.Cedula))
                return ApiResponse<ClienteDTO>.ErrorResponse("Ya existe un cliente con esa cedula.");

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

            return ApiResponse<ClienteDTO>.SuccessResponse(Mapear(nuevo), "Cliente creado.", 201);
        }

        public ApiResponse<bool> UpdateCliente(int id, UpdateClienteDTO request)
        {
            var cliente = _unitOfWork.Clientes.GetById(id);
            if (cliente == null)
                return ApiResponse<bool>.ErrorResponse("Cliente no encontrado.", 404);

            var error = Validar(request.Nombre, request.Apellido, request.Cedula, request.Correo);
            if (error != null)
                return ApiResponse<bool>.ErrorResponse(error);

            if (_unitOfWork.Clientes.GetAll().Any(c => c.Cedula == request.Cedula && c.Id != id))
                return ApiResponse<bool>.ErrorResponse("Ya existe otro cliente con esa cedula.");

            cliente.Nombre = request.Nombre;
            cliente.Apellido = request.Apellido;
            cliente.Cedula = request.Cedula;
            cliente.Telefono = request.Telefono;
            cliente.Correo = request.Correo;

            _unitOfWork.Clientes.Update(cliente);
            _unitOfWork.Complete();

            return ApiResponse<bool>.SuccessResponse(true, "Cliente actualizado.");
        }

        public ApiResponse<bool> DeleteCliente(int id)
        {
            var cliente = _unitOfWork.Clientes.GetById(id);
            if (cliente == null)
                return ApiResponse<bool>.ErrorResponse("Cliente no encontrado.", 404);

            if (_unitOfWork.Ventas.GetAll().Any(v => v.ClienteId == id))
                return ApiResponse<bool>.ErrorResponse("No se puede eliminar el cliente porque tiene ventas registradas.");

            _unitOfWork.Clientes.Delete(cliente);
            _unitOfWork.Complete();

            return ApiResponse<bool>.SuccessResponse(true, "Cliente eliminado.");
        }

        private static string? Validar(string nombre, string apellido, string cedula, string? correo)
        {
            if (string.IsNullOrWhiteSpace(nombre)) return "El nombre del cliente es obligatorio.";
            if (string.IsNullOrWhiteSpace(apellido)) return "El apellido del cliente es obligatorio.";
            if (string.IsNullOrWhiteSpace(cedula)) return "La cedula del cliente es obligatoria.";
            if (!string.IsNullOrWhiteSpace(correo) && !correo.Contains('@'))
                return "El correo no tiene un formato valido.";
            return null;
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
