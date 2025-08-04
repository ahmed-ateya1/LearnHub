using BuildingBlocks.Behaviors;
using BuildingBlocks.Exceptions.Handler;
using BuildingBlocks.Messaging.MassTransit;
using Microsoft.AspNetCore.Identity;
using Users.API.Data;
using Users.API.User.RegisterUser;

namespace Users.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();
            builder.Services.AddCarter();
            builder.Services.AddExceptionHandler<CustomExceptionHandler>();
            builder.Services.AddMediatR(cfg => {
                cfg.RegisterServicesFromAssemblyContaining<Program>();
                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            });
            builder.Services.AddValidatorsFromAssemblyContaining<RegisterCommandValidator>();

            builder.Services.AddDbContext<UserDbContext>(options =>
            {
                options.UseNpgsql(builder.Configuration.GetConnectionString("UserConnection"));
            });

            builder.Services.AddIdentity<ApplicationUser, IdentityRole<Guid>>()
                .AddEntityFrameworkStores<UserDbContext>()
                .AddDefaultTokenProviders();

            builder.Services.AddMessageBroker(builder.Configuration);

            builder.Services.AddScoped<IUserService, UserService>();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseStaticFiles();
            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapCarter();

            app.UseExceptionHandler(options => { });
            app.Run();
        }
    }
}
