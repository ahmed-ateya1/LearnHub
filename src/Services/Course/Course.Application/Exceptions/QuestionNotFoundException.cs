using BuildingBlocks.Exceptions;

namespace Course.Application.Exceptions
{
    public class QuestionNotFoundException : NotFoundException
    {
        public QuestionNotFoundException(string message) : base(message)
        {
        }
    }
}
