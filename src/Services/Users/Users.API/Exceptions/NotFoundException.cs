namespace Users.API.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string Message):base(Message)
        {
        }
    }
}
