using Course.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Course.Application.Data
{
    public interface ICourseDbContext 
    {
        DbSet<Answer> Answers { get; set; }
        DbSet<Category> Categories { get; set; }
        DbSet<Domain.Models.Course> Courses { get; set; }
        DbSet<Lecture> Lectures { get; set; }
        DbSet<Question> Questions { get; set; }
        DbSet<Quiz> Quizzes { get; set; }
        DbSet<Review> Reviews { get; set; }
        DbSet<Section> Sections { get; set; }
        DbSet<StudentAnswer> StudentAnswers { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    }
}
