using BuildingBlocks.Exceptions;

namespace Course.Application.Exceptions
{
    public class QuizNotFoundException : NotFoundException
    {
        public QuizNotFoundException(string message) : base(message)
        {
        }
    }
}
