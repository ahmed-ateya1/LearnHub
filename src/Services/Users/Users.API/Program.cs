using BuildingBlocks.Behaviors;
using BuildingBlocks.Exceptions.Handler;
using BuildingBlocks.Messaging.MassTransit;
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
                var constr = builder.Configuration.GetConnectionString("UserConnection")!
                    .Replace("$POSTGRES_HOST", builder.Configuration["POSTGRES_HOST"])
                    .Replace("$POSTGRES_USER", builder.Configuration["POSTGRES_USER"])
                    .Replace("$POSTGRES_PASSWORD", builder.Configuration["POSTGRES_PASSWORD"])
                    .Replace("$POSTGRES_DB", builder.Configuration["POSTGRES_DB"]);

                options.UseNpgsql(constr);
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
            //using (var scope = app.Services.CreateScope())
            //{
            //    var dbContext = scope.ServiceProvider.GetRequiredService<UserDbContext>();
            //    dbContext.Database.Migrate(); 
            //}


            app.UseStaticFiles();
            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapCarter();

            app.UseExceptionHandler(options => { });
            app.Run();
        }
    }
}
