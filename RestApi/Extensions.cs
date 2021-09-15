using Persistence.Models.ReadModels;
using Persistence.Models.WriteModels;
using RestApi.Models.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestApi
{
    public static class Extensions
    {
        public static TodoResponse MapToTodoResponseFromTodoRead(this TodoRead todo)
        {
            return new TodoResponse
            {
                Id = todo.Id,
                Title = todo.Title,
                Description = todo.Description,
                Difficulty = todo.Difficulty,
                DateCreated = todo.DateCreated,
                IsDone = todo.IsDone,
                UserId = todo.UserId
            };
        }

        public static TodoResponse MapToTodoResponseFromTodoWrite(this TodoWrite todo)
        {
            return new TodoResponse
            {
                Id = todo.Id,
                Title = todo.Title,
                Description = todo.Description,
                Difficulty = todo.Difficulty,
                DateCreated = todo.DateCreated,
                IsDone = todo.IsDone,
                UserId = todo.UserId
            };
        }

        public static UserResponse MapToUserResponseFromUserWrite(this UserWrite user)
        {
            return new UserResponse
            {
                Id = user.Id,
                Username = user.Username,
                Password = user.Password,
                DateCreated = user.DateCreated,
            };
        }

        public static ApiKeyResponse MapToApiKeyResponseFromApiKeyRead(this ApiKeyRead key)
        {
            return new ApiKeyResponse
            {
                Id = key.Id,
                TokenKey = key.TokenKey,
                UserId = key.UserId,
                IsActive = key.IsActive,
                DateCreated = key.DateCreated
            };
        }

        public static ApiKeyResponse MapToApiKeyResponseFromApiKeyWrite(this ApiKeyWrite key)
        {
            return new ApiKeyResponse
            {
                Id = key.Id,
                TokenKey = key.TokenKey,
                UserId = key.UserId,
                IsActive = key.IsActive,
                DateCreated = key.DateCreated
            };
        }
    }
}
