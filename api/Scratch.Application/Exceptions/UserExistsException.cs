namespace Scratch.Application.Exceptions
{
    public class UserExistsException(string email): Exception($"User with email: {email} already exists");
}
