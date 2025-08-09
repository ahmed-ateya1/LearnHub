using Course.Application.HttpClient;
using Course.Application.Mapping;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Course.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services , IConfiguration configuration)
        {
            // Register application services here
            services.AddHttpContextAccessor();
            services.AddScoped<ICourseService, CourseService>();
            services.AddScoped<IFileServices, FileService>();
            services.AddScoped<IGetUserById, GetUserById>();
            services.AddDistributedMemoryCache();
            MapsterConfig.Configure();
            services.AddHttpClient("UserService", client =>
            {
                client.BaseAddress = new Uri(
                    $"https://{configuration["UsersServiceName"]}:{configuration["UsersServicePort"]}");
            });


            //services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<DependencyInjection>());
            return services;
        }
    }
}
