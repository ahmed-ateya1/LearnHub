using BuildingBlocks.Exceptions;

namespace Course.Application.Exceptions
{
    public class CourseNotFoundException : NotFoundException
    {
        public CourseNotFoundException(string message) : base(message)
        {
        }
    }
}
