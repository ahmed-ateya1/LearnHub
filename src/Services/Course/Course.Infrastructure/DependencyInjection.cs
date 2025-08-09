using Course.Application.Data;
using Course.Domain.RepositoryContract;
using Course.Infrastructure.Data;
using Course.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Course.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services , IConfiguration configuration)
        {
            services.AddDbContext<CourseDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("CourseConnection"));
            });
            services.AddScoped<ICourseDbContext, CourseDbContext>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }
    }
}
