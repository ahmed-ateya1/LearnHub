using BuildingBlocks.Exceptions;

namespace Course.Application.Exceptions
{
    public class StudentAnswerNotFoundException : NotFoundException
    {
        public StudentAnswerNotFoundException(string message) : base(message)
        {
        }
    }
}
