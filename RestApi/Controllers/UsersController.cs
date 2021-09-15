using Microsoft.AspNetCore.Mvc;
using Persistence.Models.WriteModels;
using Persistence.Repositories;
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

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost]
        [Route("signUp")]
        public async Task<ActionResult<UserResponse>> SignUp([FromBody] AddUserRequest request)
        {
            var user = await _userRepository.GetNameAsync(request.Username);

            if (user is not null)
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
        [Route("apiKey")]
        public async Task<ActionResult<ApiKeyResponse>> CreateApiKey([FromBody] AddUserRequest request)
        {
            var user = await _userRepository.GetNamePasswordAsync(request.Username, request.Password);

            if (user is null)
            {
                return BadRequest("The username or password is incorrect");
            }

            var apiKey = new ApiKeyWrite
            {
                Id = Guid.NewGuid(),
                TokenKey = Guid.NewGuid().ToString("N"),
                UserId = user.Id,
                IsActive = true,
                DateCreated = DateTime.Now
            };

            await _userRepository.SaveApiKeyAsync(apiKey);

            return apiKey.MapToApiKeyResponseFromApiKeyWrite();
        }

        //[HttpGet]
        //[Route("apiKey")]
        //public async Task<ActionResult<IEnumerable<ApiKeyResponse>>> GetApiKeys([FromBody] AddUserRequest request)
        //{
        //    var user = await _userRepository.GetNamePasswordAsync(request.Username, request.Password);

        //    if (user is null)
        //    {
        //        return BadRequest("The username or password is incorrect");
        //    }

        //    var apiKeys = await _userRepository.GetUserApiKeysAsync(user.Id);

        //    return apiKeys.Select(apiKey => apiKey.MapToApiKeyResponseFromApiKeyRead());
        //}
    }
}
