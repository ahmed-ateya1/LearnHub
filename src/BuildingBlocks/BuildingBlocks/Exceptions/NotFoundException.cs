namespace BuildingBlocks.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message)
        {
        }
        public NotFoundException(string message, string details) : base(message)
        {
        }
    }
}
