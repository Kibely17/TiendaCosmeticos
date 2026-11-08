using Microsoft.EntityFrameworkCore;
using TiendaCosmeticos.Domain.Entities;
using TiendaCosmeticos.Infrastructure.Context;

namespace TiendaCosmeticos.Infrastructure.Repositories
{
    public class MarcaRepository : GenericRepository<Marca>
    {
        public MarcaRepository(ApplicationDbContext context) : base(context)
        {
        }

        public IEnumerable<Marca> GetAllMarcasWithProductos()
        {
            return _context.Marcas
                .Include(m => m.Productos)
                .ToList();
        }

        public bool TieneProductos(int marcaId)
        {
            return _context.Productos.Any(p => p.MarcaId == marcaId);
        }
    }
}
