using TiendaCosmeticos.Application.Core;
using TiendaCosmeticos.Application.Dtos;
using TiendaCosmeticos.Domain.Entities;
using TiendaCosmeticos.Infrastructure.Repositories;

namespace TiendaCosmeticos.Application.Services
{
    public class VentaService
    {
        private readonly UnitOfWork _unitOfWork;

        public VentaService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public ApiResponse<IEnumerable<VentaDTO>> GetAllVentas()
        {
            var ventas = _unitOfWork.Ventas.GetAll().Select(Mapear);
            return ApiResponse<IEnumerable<VentaDTO>>.SuccessResponse(ventas);
        }

        public ApiResponse<VentaDTO> GetVentaById(int id)
        {
            var venta = _unitOfWork.Ventas.GetById(id);
            if (venta == null)
                return ApiResponse<VentaDTO>.ErrorResponse("Venta no encontrada.", 404);

            return ApiResponse<VentaDTO>.SuccessResponse(Mapear(venta));
        }

        public ApiResponse<VentaDTO> CreateVenta(CreateVentaDTO request)
        {
            if (request.Cantidad <= 0)
                return ApiResponse<VentaDTO>.ErrorResponse("La cantidad debe ser mayor que cero.");

            if (_unitOfWork.Clientes.GetById(request.ClienteId) == null)
                return ApiResponse<VentaDTO>.ErrorResponse("El cliente indicado no existe.");

            var producto = _unitOfWork.Productos.GetById(request.ProductoId);
            if (producto == null)
                return ApiResponse<VentaDTO>.ErrorResponse("El producto indicado no existe.");

            if (producto.Stock < request.Cantidad)
                return ApiResponse<VentaDTO>.ErrorResponse($"Stock insuficiente. Disponible: {producto.Stock}.");

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
                return ApiResponse<VentaDTO>.ErrorResponse("No se pudo registrar la venta.", 500);
            }

            return ApiResponse<VentaDTO>.SuccessResponse(Mapear(venta), "Venta registrada.", 201);
        }

        public ApiResponse<bool> UpdateVenta(int id, UpdateVentaDTO request)
        {
            var venta = _unitOfWork.Ventas.GetById(id);
            if (venta == null)
                return ApiResponse<bool>.ErrorResponse("Venta no encontrada.", 404);

            if (request.Cantidad <= 0)
                return ApiResponse<bool>.ErrorResponse("La cantidad debe ser mayor que cero.");

            venta.Cantidad = request.Cantidad;
            venta.Total = venta.PrecioUnitario * request.Cantidad;

            _unitOfWork.Ventas.Update(venta);
            _unitOfWork.Complete();

            return ApiResponse<bool>.SuccessResponse(true, "Venta actualizada.");
        }

        public ApiResponse<bool> DeleteVenta(int id)
        {
            var venta = _unitOfWork.Ventas.GetById(id);
            if (venta == null)
                return ApiResponse<bool>.ErrorResponse("Venta no encontrada.", 404);

            _unitOfWork.Ventas.Delete(venta);
            _unitOfWork.Complete();

            return ApiResponse<bool>.SuccessResponse(true, "Venta eliminada.");
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
