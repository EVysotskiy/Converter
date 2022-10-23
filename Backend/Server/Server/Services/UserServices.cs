using Domain.Model;
using Domain.Services;
using Server.Repositories;

namespace Server.Services
{
    public class UserServices : IUserServices
    {
        
        private readonly UserRepository _userRepository;
        private readonly ILogger _logger;

        public UserServices
        (
            UserRepository userRepository, 
            ILogger<UserServices> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }
        
        public async Task<User> Get(long chatId)
        {
            return await _userRepository.First(user => user.IdChat == chatId);
        }
        
        public async Task<User> Add(User newUser)
        {
            return await _userRepository.Add(newUser);
        }
    }
}