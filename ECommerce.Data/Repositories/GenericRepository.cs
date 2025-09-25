using ECommerce.Data.Interfaces;
using ECommerce.Data.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace RabiesMessagesService.Data.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<TEntity> _dbSet;


        public GenericRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }


        public async Task AddAsync(TEntity entity) => await _dbSet.AddAsync(entity);
        public async Task<IEnumerable<TEntity>> GetAllAsync() => await _dbSet.ToListAsync();
        public async Task<TEntity?> GetByIdAsync(int id) => await _dbSet.FindAsync(id);
        public void Remove(TEntity entity) => _dbSet.Remove(entity);
        public void Update(TEntity entity) => _dbSet.Update(entity);
    }
}
