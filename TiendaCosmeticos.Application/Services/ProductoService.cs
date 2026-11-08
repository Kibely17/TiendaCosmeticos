using TiendaCosmeticos.Application.Core;
using TiendaCosmeticos.Application.Dtos;
using TiendaCosmeticos.Domain.Entities;
using TiendaCosmeticos.Infrastructure.Repositories;

namespace TiendaCosmeticos.Application.Services
{
    public class ProductoService
    {
        private readonly UnitOfWork _unitOfWork;

        public ProductoService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public ApiResponse<IEnumerable<ProductoDTO>> GetAllProductos()
        {
            var productos = _unitOfWork.Productos.GetAllProductosWithMarca().Select(Mapear);
            return ApiResponse<IEnumerable<ProductoDTO>>.SuccessResponse(productos);
        }

        public ApiResponse<ProductoDTO> GetProductoById(int id)
        {
            var producto = _unitOfWork.Productos.GetById(id);
            if (producto == null)
                return ApiResponse<ProductoDTO>.ErrorResponse("Producto no encontrado.", 404);

            return ApiResponse<ProductoDTO>.SuccessResponse(Mapear(producto));
        }

        public ApiResponse<ProductoDTO> CreateProducto(CreateProductoDTO request)
        {
            var error = Validar(request.Nombre, request.Precio, request.Stock, request.MarcaId);
            if (error != null)
                return ApiResponse<ProductoDTO>.ErrorResponse(error);

            var nuevo = new Producto
            {
                Nombre = request.Nombre,
                Precio = request.Precio,
                Stock = request.Stock,
                MarcaId = request.MarcaId
            };

            _unitOfWork.Productos.Create(nuevo);
            _unitOfWork.Complete();

            return ApiResponse<ProductoDTO>.SuccessResponse(Mapear(nuevo), "Producto creado.", 201);
        }

        public ApiResponse<bool> UpdateProducto(int id, UpdateProductoDTO request)
        {
            var producto = _unitOfWork.Productos.GetById(id);
            if (producto == null)
                return ApiResponse<bool>.ErrorResponse("Producto no encontrado.", 404);

            var error = Validar(request.Nombre, request.Precio, request.Stock, request.MarcaId);
            if (error != null)
                return ApiResponse<bool>.ErrorResponse(error);

            producto.Nombre = request.Nombre;
            producto.Precio = request.Precio;
            producto.Stock = request.Stock;
            producto.MarcaId = request.MarcaId;

            _unitOfWork.Productos.Update(producto);
            _unitOfWork.Complete();

            return ApiResponse<bool>.SuccessResponse(true, "Producto actualizado.");
        }

        public ApiResponse<bool> DeleteProducto(int id)
        {
            var producto = _unitOfWork.Productos.GetById(id);
            if (producto == null)
                return ApiResponse<bool>.ErrorResponse("Producto no encontrado.", 404);

            _unitOfWork.Productos.Delete(producto);
            _unitOfWork.Complete();

            return ApiResponse<bool>.SuccessResponse(true, "Producto eliminado.");
        }

        private string? Validar(string nombre, decimal precio, int stock, int marcaId)
        {
            if (string.IsNullOrWhiteSpace(nombre)) return "El nombre del producto es obligatorio.";
            if (precio <= 0) return "El precio debe ser mayor que cero.";
            if (stock < 0) return "El stock no puede ser negativo.";
            if (_unitOfWork.Marcas.GetById(marcaId) == null) return "La marca indicada no existe.";
            return null;
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
