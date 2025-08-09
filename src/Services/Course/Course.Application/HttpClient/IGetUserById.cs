namespace Course.Application.HttpClient
{
    public interface IGetUserById
    {
        Task<UserDto?> ExecuteAsync(Guid userId);
    }
}
