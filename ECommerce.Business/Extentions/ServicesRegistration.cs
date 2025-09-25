
using ECommerce.Business.Interfaces;
using ECommerce.Business.Options;
using ECommerce.Business.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ECommerce.Business.Extentions
{
    public static partial class ServicesRegistration
    {
        public static WebApplicationBuilder AddBusinessServicesRegistration(this WebApplicationBuilder builder)
        {
            builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(JwtOptions.SectionName));
            builder.Services.Configure<FileSettings>(builder.Configuration.GetSection(FileSettings.SectionName));

            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<IFileService, FileService>();
            return builder;
        }
    }
}
