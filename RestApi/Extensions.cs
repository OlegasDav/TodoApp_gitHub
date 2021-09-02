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
                IsDone = todo.IsDone
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
                IsDone = todo.IsDone
            };
        }
    }
}
