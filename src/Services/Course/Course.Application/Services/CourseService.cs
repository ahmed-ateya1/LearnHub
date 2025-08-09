using Course.Application.HttpClient;
using Course.Domain.Models;
using Mapster;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace Course.Application.Services
{
    public class CourseService(
        IUnitOfWork unitOfWork,
        IFileServices fileService,
        ILogger<CourseService> logger,
        IGetUserById getUser) : ICourseService
    {
        private async Task ExecuteWithTransactionAsync(Func<Task> action)
        {
            using var transaction = await unitOfWork.BeginTransactionAsync();
            try
            {
                await action();
                await unitOfWork.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while executing transaction");
                await unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        private async Task<UserDto?> GetInstructorById(Guid userId)
        {
            var user = await getUser.ExecuteAsync(userId);
            if (user == null)
                throw new InstructorNotFoundException("Instructor not found");

            return user;
        }

        private async Task<IEnumerable<CourseResponse>> PrepareResponse(IEnumerable<Domain.Models.Course> courses)
        {
            var responses = courses.Adapt<List<CourseResponse>>();

            var distinctInstructorIds = responses.Select(r => r.InstructorId).Distinct();
            var instructorCache = new Dictionary<Guid, UserDto?>();

            foreach (var id in distinctInstructorIds)
            {
                instructorCache[id] = await getUser.ExecuteAsync(id);
            }

            foreach (var r in responses)
            {
                var inst = instructorCache[r.InstructorId];
                r.InstructorName = inst != null ? $"{inst.FirstName} {inst.LastName}" : "Unknown";
                r.CategoryName = courses.First(c => c.Id == r.Id).Category?.Name ?? "Unknown";
            }

            return responses;
        }

        public async Task<CourseResponse> CreateCourseAsync(CourseAddRequest courseAddRequest)
        {
            if (courseAddRequest == null)
                throw new ArgumentNullException(nameof(courseAddRequest));

           logger.LogInformation("Creating course with title: {Title}", courseAddRequest.Title);

            var instructor = await GetInstructorById(courseAddRequest.InstructorId);



            var category = await unitOfWork.Repository<Category>()
                .GetByAsync(c => c.Id == courseAddRequest.CategoryId)
                ?? throw new CategoryNotFoundException("Category not found");

            var course = courseAddRequest.Adapt<Domain.Models.Course>();

            logger.LogInformation("Mapped course from request: {Course}", course);

            if (courseAddRequest.Poster != null)
            {
                logger.LogInformation("Uploading poster for course: {Title}", courseAddRequest.Title);
                course.PosterUrl = await fileService.CreateFile(courseAddRequest.Poster);
            }

            await ExecuteWithTransactionAsync(async () =>
            {
                await unitOfWork.Repository<Domain.Models.Course>().CreateAsync(course);
            });

            logger.LogInformation("Course created successfully with ID: {CourseId}", course.Id);
            var courseResponse = course.Adapt<CourseResponse>();
            courseResponse.InstructorName = $"{instructor.FirstName} {instructor.LastName}";
            courseResponse.CategoryName = category.Name;

            return courseResponse;
        }

        public async Task<bool> DeleteCourseAsync(Guid id)
        {
            logger.LogInformation("Deleting course with ID: {CourseId}", id);
            var course = await unitOfWork.Repository<Domain.Models.Course>()
                .GetByAsync(c => c.Id == id)
                ?? throw new CourseNotFoundException("Course not found");

            logger.LogInformation("Found course to delete: {Course}", course);
            await ExecuteWithTransactionAsync(async () =>
            {
                await unitOfWork.Repository<Domain.Models.Course>().DeleteAsync(course);
            });

            logger.LogInformation("Course deleted successfully with ID: {CourseId}", id);
            return true;
        }

        public async Task<IEnumerable<CourseResponse>> GetAllCoursesAsync(Expression<Func<Domain.Models.Course, bool>>? filter = null)
        {
            var courses = await unitOfWork.Repository<Domain.Models.Course>()
                .GetAllAsync(filter, includeProperties: "Category,Sections,Reviews");

            logger.LogInformation("Retrieved {Count} courses from the database", courses?.Count() ?? 0);
            if (courses == null || !courses.Any())
                return Enumerable.Empty<CourseResponse>();

            return await PrepareResponse(courses);
        }

        public async Task<CourseResponse> GetCourseByAsync(Expression<Func<Domain.Models.Course, bool>> filter)
        {
            logger.LogInformation("Fetching course with filter: {Filter}", filter);
            var course = await unitOfWork.Repository<Domain.Models.Course>()
               .GetByAsync(filter, includeProperties: "Category,Sections,Reviews")
               ?? throw new CourseNotFoundException("Course not found");

            var response = course.Adapt<CourseResponse>();

            var inst = await GetInstructorById(response.InstructorId);
            response.InstructorName = $"{inst.FirstName} {inst.LastName}";

            logger.LogInformation("Course fetched successfully: {CourseId}", response.Id);
            return response;
        }

        public async Task<CourseResponse> UpdateCourseAsync(CourseUpdateRequest courseUpdateRequest)
        {
            if (courseUpdateRequest == null)
                throw new ArgumentNullException(nameof(courseUpdateRequest));

            logger.LogInformation("Updating course with ID: {CourseId}", courseUpdateRequest.Id);

            var course = await unitOfWork.Repository<Domain.Models.Course>()
              .GetByAsync(x => x.Id == courseUpdateRequest.Id, includeProperties: "Category,Sections,Reviews")
              ?? throw new CourseNotFoundException("Course not found");

            await GetInstructorById(course.InstructorId);

            courseUpdateRequest.Adapt(course);

            await ExecuteWithTransactionAsync(async () =>
            {
                logger.LogInformation("Updating course details: {Course}", course);
                if (courseUpdateRequest.Poster != null)
                {
                    course.PosterUrl = await fileService.UpdateFile(courseUpdateRequest.Poster, course.PosterUrl);
                }
                await unitOfWork.Repository<Domain.Models.Course>().UpdateAsync(course);
            });

            var response = course.Adapt<CourseResponse>();
            logger.LogInformation("Course updated successfully: {CourseId}", response.Id);
            return response;
        }
    }
}
