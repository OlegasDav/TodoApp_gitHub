using Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Domain.Services
{
    public class ApiKeyService : IApiKeyService
    {
        private readonly IUserRepository _userRepository;
        private readonly int _apiKeyLimit;

        public ApiKeyService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _apiKeyLimit = Convert.ToInt32(configuration.GetSection("AppSettings")["ApiKeyLimits"]);
        }

        public async Task<bool> CheckApiKeyLimitAsync(Guid userId)
        {
            var apiKeys = await _userRepository.GetUserApiKeysAsync(userId);

            var apiKeyCount = apiKeys.ToList().Count;

            if (_apiKeyLimit > apiKeyCount)
            {
                return false;
            }

            return true;
        }
    }
}
