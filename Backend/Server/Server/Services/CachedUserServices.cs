using System.Threading.Tasks;
using Domain.Model;
using Domain.Models;
using Domain.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Server.Extensions;

namespace Server.Services
{
    public class CachedUserServices : IUserServices
    {
        private readonly IUserServices _userServices;
        private const string KeyPrefix = "users";
        private readonly IMemoryCache _cache;
        private readonly ILogger<CachedUserServices> _logger;

        public CachedUserServices(IUserServices userServices, IMemoryCache cache, ILogger<CachedUserServices> logger)
        {
            _userServices = userServices;
            _cache = cache;
            _logger = logger;
        }
        
        public async Task<User> Get(long chatId)
        {
            _logger.Log(LogLevel.Information,$"Get User {KeyPrefix}:{chatId}");
            return await _cache.Remember($"{KeyPrefix}:{chatId}", async () => await _userServices.Get(chatId));
        }
        
        public async Task<User> Add(User newUser)
        {
            await _userServices.Add(newUser);
            _logger.Log(LogLevel.Information,$"Add User {KeyPrefix}:{newUser.IdChat}");
            _cache.Set($"{KeyPrefix}:{newUser.IdChat}", newUser);
            return newUser;
        }
    }
}