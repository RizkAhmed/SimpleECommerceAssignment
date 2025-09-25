
using ECommerce.Data.Entities;

namespace ECommerce.Data.Interfaces
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<Product?> GetByCodeAsync(string code);
    }
}