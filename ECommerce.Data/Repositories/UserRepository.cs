using Azure.Core;
using ECommerce.Data.Entities;
using ECommerce.Data.Interfaces;
using ECommerce.Data.Persistence;
using Microsoft.EntityFrameworkCore;

namespace RabiesMessagesService.Data.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context) { }

        public async Task<User?> GetByUserNameAndPasswordAsync(string userName, string password) => await _dbSet.SingleOrDefaultAsync(x => x.UserName == userName && x.Password == password);
        public async Task<User?> GetByEmailAsync(string email) => await _dbSet.SingleOrDefaultAsync(x => x.Email == email);
        public async Task<User?> GetByUserNameAsync(string userName) => await _dbSet.SingleOrDefaultAsync(x => x.UserName == userName);
    }
}
