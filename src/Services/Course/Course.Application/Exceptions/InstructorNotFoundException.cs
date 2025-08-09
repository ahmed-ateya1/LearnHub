using BuildingBlocks.Exceptions;

namespace Course.Application.Exceptions
{
    public class InstructorNotFoundException : NotFoundException
    {
        public InstructorNotFoundException(string message) : base(message)
        {
        }
    }
}
