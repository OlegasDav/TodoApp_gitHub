using Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Persistence.Models.WriteModels;
using Persistence.Repositories;
using RestApi.Attributes;
using RestApi.Models.RequestModels;
using RestApi.Models.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestApi.Controllers
{
    [ApiController]
    
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IApiKeyService _apiKeyService;

        public UsersController(IUserRepository userRepository, IApiKeyService apiKeyService)
        {
            _userRepository = userRepository;
            _apiKeyService = apiKeyService;
        }

        [HttpPost]
        [Route("signUp")]
        public async Task<ActionResult<UserResponse>> SignUp([FromBody] AddUserRequest request)
        {
            var isExists = await _userRepository.CheckExistAsync(request.Username);

            if (isExists == 1)
            {
                return BadRequest($"A user with {request.Username} name already exists");
            }

            var userNew = new UserWrite
            {
                Id = Guid.NewGuid(),
                Username = request.Username,
                Password = request.Password,
                DateCreated = DateTime.Now,
            };

            await _userRepository.SaveUserAsync(userNew);

            return userNew.MapToUserResponseFromUserWrite();
        }

        [HttpPost]
        [Route("signIn")]
        public async Task<ActionResult<UserTokenResponse>> SignIn([FromBody] AddUserRequest request)
        {
            var user = await _userRepository.GetNamePasswordAsync(request.Username, request.Password);

            if (user is null)
            {
                return BadRequest("The username or password is incorrect");
            }

            var userTokenNew = new UserTokenWrite
            {
                UserId = user.Id,
                Token = Guid.NewGuid().ToString("N"),
                ExpirationDate = DateTime.Now.AddMinutes(5)
            };

            await _userRepository.SaveOrUpadteTokenAsync(userTokenNew);

            return userTokenNew.MapToUserTokenResponseFromUserTokenWrite();
        }

        [HttpPost]
        [UserToken]
        [Route("apiKey")]
        public async Task<ActionResult<ApiKeyResponse>> CreateApiKey()
        {
            var userId = (Guid)HttpContext.Items["userId"];
            var isReachedLimit = await _apiKeyService.CheckApiKeyLimitAsync(userId);

            if (isReachedLimit)
            {
                return Conflict("You have exceeded the ApiKey limit");
            }

            var apiKey = new ApiKeyWrite
            {
                Id = Guid.NewGuid(),
                TokenKey = Guid.NewGuid().ToString("N"),
                UserId = userId,
                IsActive = true,
                DateCreated = DateTime.Now
            };

            await _userRepository.SaveApiKeyAsync(apiKey);

            return apiKey.MapToApiKeyResponseFromApiKeyWrite();
        }

        [HttpGet]
        [UserToken]
        [Route("apiKey")]
        public async Task<ActionResult<IEnumerable<ApiKeyResponse>>> GetApiKeys()
        {
            var userId = (Guid)HttpContext.Items["userId"];

            var apiKeys = await _userRepository.GetUserApiKeysAsync(userId);

            return Ok(apiKeys.Select(apiKey => apiKey.MapToApiKeyResponseFromApiKeyRead()));
        }
    }
}
