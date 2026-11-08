using Microsoft.EntityFrameworkCore;
using TiendaCosmeticos.Domain.Entities;
using TiendaCosmeticos.Infrastructure.Context;

namespace TiendaCosmeticos.Infrastructure.Repositories
{
    public class ProductoRepository : GenericRepository<Producto>
    {
        public ProductoRepository(ApplicationDbContext context) : base(context)
        {
        }

        public IEnumerable<Producto> GetAllProductosWithMarca()
        {
            return _context.Productos
                .Include(p => p.Marca)
                .ToList();
        }

        public Producto? GetProductoWithMarca(int id)
        {
            return _context.Productos
                .Include(p => p.Marca)
                .FirstOrDefault(p => p.Id == id);
        }
    }
}
