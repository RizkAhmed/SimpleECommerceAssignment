
using ECommerce.Data.Entities;

namespace ECommerce.Data.Interfaces
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User?> GetByUserNameAndPasswordAsync(string userName,string password);
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByUserNameAsync(string name);
    }
}