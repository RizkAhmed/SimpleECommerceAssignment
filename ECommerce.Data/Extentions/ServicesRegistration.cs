using ECommerce.Data.Interfaces;
using ECommerce.Data.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabiesMessagesService.Data.Repositories;

namespace ECommerce.Data.Extentions
{
    public static partial class ServicesRegistration
    {
        public static WebApplicationBuilder AddDataServicesRegistration(this WebApplicationBuilder builder)
        {

            // DbContext
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString));

            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            return builder;
        }
    }
}
