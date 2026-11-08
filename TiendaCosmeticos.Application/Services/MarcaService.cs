using TiendaCosmeticos.Application.Core;
using TiendaCosmeticos.Application.Dtos;
using TiendaCosmeticos.Domain.Entities;
using TiendaCosmeticos.Infrastructure.Repositories;

namespace TiendaCosmeticos.Application.Services
{
    public class MarcaService
    {
        private readonly UnitOfWork _unitOfWork;

        public MarcaService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public ApiResponse<IEnumerable<MarcaDTO>> GetAllMarcas()
        {
            var marcas = _unitOfWork.Marcas.GetAll().Select(Mapear);
            return ApiResponse<IEnumerable<MarcaDTO>>.SuccessResponse(marcas);
        }

        public ApiResponse<MarcaDTO> GetMarcaById(int id)
        {
            var marca = _unitOfWork.Marcas.GetById(id);
            if (marca == null)
                return ApiResponse<MarcaDTO>.ErrorResponse("Marca no encontrada.", 404);

            return ApiResponse<MarcaDTO>.SuccessResponse(Mapear(marca));
        }

        public ApiResponse<MarcaDTO> CreateMarca(CreateMarcaDTO request)
        {
            if (string.IsNullOrWhiteSpace(request.Nombre))
                return ApiResponse<MarcaDTO>.ErrorResponse("El nombre de la marca es obligatorio.");

            var nueva = new Marca { Nombre = request.Nombre, Pais = request.Pais };

            _unitOfWork.Marcas.Create(nueva);
            _unitOfWork.Complete();

            return ApiResponse<MarcaDTO>.SuccessResponse(Mapear(nueva), "Marca creada.", 201);
        }

        public ApiResponse<bool> UpdateMarca(int id, UpdateMarcaDTO request)
        {
            var marca = _unitOfWork.Marcas.GetById(id);
            if (marca == null)
                return ApiResponse<bool>.ErrorResponse("Marca no encontrada.", 404);

            if (string.IsNullOrWhiteSpace(request.Nombre))
                return ApiResponse<bool>.ErrorResponse("El nombre de la marca es obligatorio.");

            marca.Nombre = request.Nombre;
            marca.Pais = request.Pais;

            _unitOfWork.Marcas.Update(marca);
            _unitOfWork.Complete();

            return ApiResponse<bool>.SuccessResponse(true, "Marca actualizada.");
        }

        public ApiResponse<bool> DeleteMarca(int id)
        {
            var marca = _unitOfWork.Marcas.GetById(id);
            if (marca == null)
                return ApiResponse<bool>.ErrorResponse("Marca no encontrada.", 404);

            if (_unitOfWork.Marcas.TieneProductos(id))
                return ApiResponse<bool>.ErrorResponse("No se puede eliminar la marca porque tiene productos asociados.");

            _unitOfWork.Marcas.Delete(marca);
            _unitOfWork.Complete();

            return ApiResponse<bool>.SuccessResponse(true, "Marca eliminada.");
        }

        private static MarcaDTO Mapear(Marca m) => new MarcaDTO
        {
            Id = m.Id,
            Nombre = m.Nombre,
            Pais = m.Pais
        };
    }
}
