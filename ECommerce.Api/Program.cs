
using ECommerce.Api.Extentions;
using ECommerce.Business.Extentions;
using ECommerce.Data.Extentions;

namespace ECommerce.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                                  policy =>
                                  {
                                      policy.AllowAnyOrigin() // <-- This is the change
                                            .AllowAnyHeader()
                                            .AllowAnyMethod();
                                  });
            });



            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddOpenApi();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.AddApiServicesRegistration();
            builder.AddBusinessServicesRegistration();
            builder.AddDataServicesRegistration();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();

                app.UseSwagger();
                app.UseSwaggerUI();

            }
           
            app.UseHttpsRedirection();
            app.UseCors(MyAllowSpecificOrigins);

            app.UseAuthorization();

            app.MapStaticAssets();

            app.MapControllers();

            app.Run();
        }
    }
}
