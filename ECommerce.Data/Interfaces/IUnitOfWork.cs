using ECommerce.Data.Entities;

namespace ECommerce.Data.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class;
        IUserRepository Users { get; }
        IProductRepository Products { get; }
        Task<int> SaveChangesAsync();
    }
}