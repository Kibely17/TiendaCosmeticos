using Microsoft.EntityFrameworkCore.Storage;
using TiendaCosmeticos.Domain.Entities;
using TiendaCosmeticos.Infrastructure.Context;

namespace TiendaCosmeticos.Infrastructure.Repositories
{
    public class UnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IDbContextTransaction? _transaction;

        public UnitOfWork(
            ApplicationDbContext context,
            MarcaRepository marcas,
            ProductoRepository productos,
            GenericRepository<Cliente> clientes,
            GenericRepository<Venta> ventas)
        {
            _context = context;
            Marcas = marcas;
            Productos = productos;
            Clientes = clientes;
            Ventas = ventas;
        }

        public MarcaRepository Marcas { get; }

        public ProductoRepository Productos { get; }

        public GenericRepository<Cliente> Clientes { get; }

        public GenericRepository<Venta> Ventas { get; }

        public void Complete()
        {
            _context.SaveChanges();
        }

        public void BeginTransaction()
        {
            _transaction = _context.Database.BeginTransaction();
        }

        public void CommitTransaction()
        {
            _transaction?.Commit();
            _transaction = null;
        }

        public void RollbackTransaction()
        {
            _transaction?.Rollback();
            _transaction = null;
        }
    }
}
