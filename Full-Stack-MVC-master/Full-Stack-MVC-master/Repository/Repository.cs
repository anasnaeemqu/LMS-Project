using Microsoft.EntityFrameworkCore;
using mvcLab.Models;
using mvcLab.Repository.IRepository;
using NuGet.Versioning;

namespace mvcLab.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext context;
        private readonly DbSet<T> dbSet;

        public Repository(ApplicationDbContext _context)
        {
            context = _context;
            dbSet = context.Set<T>();
        }

        public void Add(T entity)
        {
            dbSet.Add(entity);
            context.SaveChanges();
        } 
        public void Update(T entity)
        {
            dbSet.Update(entity);
            context.SaveChanges();
        }

        public IEnumerable<T> GetAll(string? includeProperties = null)
        {
            IQueryable<T> query =dbSet;
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProp in includeProperties
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            return query.ToList();
        }

        public T GetById(int id, string? includeProperties = null)
        {
                IQueryable<T> query =dbSet;
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProp in includeProperties
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            return query.FirstOrDefault(e => EF.Property<int>(e, "Id") == id);
        }

        public void Remove(T entity)
        {
            dbSet.Remove(entity);
            context.SaveChanges();
        }
        public void RemoveRange(IEnumerable<T> entities)
        {
            dbSet.RemoveRange(entities);
            context.SaveChanges();
        }
    }

}
