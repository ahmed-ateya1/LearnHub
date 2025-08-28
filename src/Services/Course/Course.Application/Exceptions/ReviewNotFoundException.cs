using BuildingBlocks.Exceptions;

namespace Course.Application.Exceptions
{
    public class ReviewNotFoundException : NotFoundException
    {
        public ReviewNotFoundException(string message) : base(message)
        {
        }
    }
}
