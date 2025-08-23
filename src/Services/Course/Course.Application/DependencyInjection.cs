using BuildingBlocks.Behaviors;
using Course.Application.HttpClient;
using Course.Application.Mapping;
using Course.Application.Slices.Courses.Commands.CreateCourse;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Course.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            // Register application services here
            services.AddHttpContextAccessor();
            services.AddScoped<ICourseService, CourseService>();
            services.AddScoped<IFileServices, FileService>();
            services.AddScoped<IGetUserById, GetUserById>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ISectionService, SectionService>();
            services.AddScoped<ILectureService, LectureService>();
            services.AddScoped<IQuizService, QuizService>();
            services.AddScoped<IQuestionService, QuestionService>();
            services.AddScoped<IStudentAnswerService, StudentAnswerService>();  
            services.AddDistributedMemoryCache();
            MapsterConfig.Configure();

            // Add debugging to see what values are being used
          

            services.AddHttpClient("UserService", client =>
            {
                var serviceName = configuration["UsersServiceName"] ?? "usersapi";
                var servicePort = configuration["UsersServicePort"] ?? "8080";
                var baseAddress = $"http://{serviceName}:{servicePort}";
                client.BaseAddress = new Uri(baseAddress);

                Console.WriteLine($"HttpClient configured with base address: {baseAddress}");
            });

            services.AddFluentValidationAutoValidation();

            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblyContaining<CreateCourseCommand>();
                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            });
            return services;
        }
    }
}