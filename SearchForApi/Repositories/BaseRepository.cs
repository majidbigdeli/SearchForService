using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System;
using SearchForApi.Models.DatabaseContext;
using SearchForApi.Models.Entities;
using System.Linq;
using System.Linq.Expressions;

namespace SearchForApi.Repositories
{
    public class BaseRepository<T, Q>
        where T : BaseEntity<Q>
    {
        private readonly ApiContext _context;
        public DbSet<T> _entities;

        public BaseRepository(ApiContext context)
        {
            _context = context;
            _entities = context.Set<T>();
        }

        public IQueryable<T> Where(Expression<Func<T, bool>> predicate)
        {
            return _entities.Where(predicate);
        }

        public IQueryable<T> Get()
        {
            return _entities;
        }

        public virtual Task<T> Get(Q id)
        {
            throw new NotImplementedException();
        }

        public virtual Task<bool> Exist(Q id)
        {
            throw new NotImplementedException();
        }

        public void Add(T entity)
        {
            _entities.Add(entity);
        }

        public Task Insert(T entity)
        {
            _entities.Add(entity);
            return _context.SaveChangesAsync();
        }

        public Task SaveChanges()
        {
            return _context.SaveChangesAsync();
        }

        public Task Delete(T entity)
        {
            _entities.Remove(entity);
            return _context.SaveChangesAsync();
        }
    }
}