
using BuildingBlocks.Exceptions.Handler;
using BuildingBlocks.Messaging.MassTransit;
using Carter;
using Enrollement.API.Data;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Enrollement.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddDbContext<EnrollementDbContext>(options =>
            {
                options.UseNpgsql(builder.Configuration.GetConnectionString("EnrollementConnection"));
            });


            builder.Services.AddMessageBroker(builder.Configuration, Assembly.GetExecutingAssembly());

            builder.Services.AddCarter();

            builder.Services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblyContaining<Program>();
            });
            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            builder.Services.AddExceptionHandler<CustomExceptionHandler>();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapCarter();
            app.UseExceptionHandler(opt => { });

            app.Run();
        }
    }
}
