using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BuildingBlocks.Messaging.MassTransit
{
    public static class Extension
    {
        public static IServiceCollection AddMessageBroker(this IServiceCollection services, IConfiguration configuration, Assembly? assembly = null)
        {
            services.AddMassTransit(config =>
            {
                config.SetKebabCaseEndpointNameFormatter();
                if (assembly != null)
                    config.AddConsumers(assembly);

                config.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(configuration["MessageBroker:Host"], h =>
                    {
                        h.Username(configuration["MessageBroker:UserName"]!);
                        h.Password(configuration["MessageBroker:Password"]!);
                    });
                    cfg.ConfigureEndpoints(context);
                });
            });
            return services;
        }
    }
}
