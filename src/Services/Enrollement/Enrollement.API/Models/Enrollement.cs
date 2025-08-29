namespace Enrollement.API.Models
{
    public class Enrollement
    {
        public Guid Id { get; set; } 
        public Guid CourseId { get; set; }
        public Guid StudentId { get; set; }
        public DateTime EnrollementDate { get; set; }
    }
}
