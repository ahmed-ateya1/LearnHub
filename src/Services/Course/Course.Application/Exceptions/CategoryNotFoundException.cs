using BuildingBlocks.Exceptions;

namespace Course.Application.Exceptions
{
    public class CategoryNotFoundException : NotFoundException
    {
        public CategoryNotFoundException(string message) : base(message)
        {
        }
    }
}
