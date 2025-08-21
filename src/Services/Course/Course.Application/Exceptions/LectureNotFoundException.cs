using BuildingBlocks.Exceptions;

namespace Course.Application.Exceptions
{
    public class LectureNotFoundException : NotFoundException
    {
        public LectureNotFoundException(string msg) : base(msg)
        {

        }
        
    }
}
