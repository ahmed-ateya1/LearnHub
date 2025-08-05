using BuildingBlocks.Exceptions.Handler;
using BuildingBlocks.Messaging.MassTransit;
using Notification.API.Hubs;
using System.Reflection;
using JasperFx;

namespace Notification.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add OpenAPI services
            builder.Services.AddOpenApi();

            builder.Services.AddMarten(opts =>
            {
                var connectionString = builder.Configuration.GetConnectionString("NotificationConnection")!;
                opts.Connection(connectionString);
               
                opts.AutoCreateSchemaObjects = AutoCreate.All;
                opts.DatabaseSchemaName = "notification";
            }).UseLightweightSessions();

            builder.Services.AddMessageBroker(builder.Configuration, Assembly.GetExecutingAssembly());

            builder.Services.AddCarter();

            builder.Services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblyContaining<Program>();
            });

            builder.Services.AddSignalR();

            builder.Services.AddExceptionHandler<CustomExceptionHandler>();

            builder.Services.AddAuthorization();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
                app.MapOpenApi();

            app.UseHttpsRedirection();
            app.UseAuthorization();

            app.MapCarter();
            app.MapHub<NotificationHub>("/notification");

            app.UseExceptionHandler(opt => { });

            app.Run();
        }
    }
}
