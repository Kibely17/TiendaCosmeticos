using Microsoft.EntityFrameworkCore;
using TiendaCosmeticos.Domain.Core;
using TiendaCosmeticos.Infrastructure.Context;

namespace TiendaCosmeticos.Infrastructure.Repositories
{
    public class GenericRepository<T> where T : BaseEntity
    {
        protected readonly ApplicationDbContext _context;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }

        public T? GetById(int id)
        {
            return _context.Set<T>().FirstOrDefault(e => e.Id == id);
        }

        public void Create(T entity)
        {
            entity.Created = DateTime.Now;
            _context.Set<T>().Add(entity);
        }

        public void Update(T entity)
        {
            entity.Updated = DateTime.Now;
            _context.Set<T>().Update(entity);
        }

        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }
    }
}
