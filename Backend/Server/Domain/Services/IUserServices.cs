using Domain.Model;

namespace Domain.Services;

public interface IUserServices
{
    Task<User> Get(long chatId);
    Task<User> Add(User newUser);
}