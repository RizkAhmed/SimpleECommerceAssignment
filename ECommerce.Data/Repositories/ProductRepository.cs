using ECommerce.Data.Entities;
using ECommerce.Data.Interfaces;
using ECommerce.Data.Persistence;
using Microsoft.EntityFrameworkCore;

namespace RabiesMessagesService.Data.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext context) : base(context) { }
        public async Task<Product?> GetByCodeAsync(string code) => await _dbSet.SingleOrDefaultAsync(p => p.ProductCode == code);
    }
}
