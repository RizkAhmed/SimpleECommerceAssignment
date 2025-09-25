using ECommerce.Data.Entities;
using ECommerce.Data.Interfaces;
using ECommerce.Data.Persistence;
using System.Collections.Concurrent;

namespace RabiesMessagesService.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private readonly ConcurrentDictionary<Type, object> _repos = new();
        public UnitOfWork(AppDbContext context, IUserRepository users, IProductRepository products)
        {
            _context = context;
            Users = users;
            Products = products;
        }
        public IUserRepository Users { get; }
        public IProductRepository Products { get; }
        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class
        {
            var type = typeof(TEntity);
            if (!_repos.TryGetValue(type, out var repo))
            {
                repo = new GenericRepository<TEntity>(_context);
                _repos[type] = repo;
            }
            return (IGenericRepository<TEntity>)repo;
        }
        public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();
        public void Dispose() => _context.Dispose();
    }
}
