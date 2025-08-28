using BuildingBlocks.Exceptions.Handler;
using Course.Application;
using Course.Infrastructure;
using BuildingBlocks.Messaging.MassTransit;

namespace Course.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services
                .AddInfrastructure(builder.Configuration)
                .AddApplication(builder.Configuration);

            builder.Services.AddMessageBroker(builder.Configuration);
            builder.Services.AddExceptionHandler<CustomExceptionHandler>();
                

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.UseStaticFiles();

            app.UseExceptionHandler(options => { });
            app.Run();
        }
    }
}
