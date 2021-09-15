using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Persistence.Models.WriteModels;
using Persistence.Repositories;
using RestApi.Attributes;
using RestApi.Models;
using RestApi.Models.RequestModels;
using RestApi.Models.ResponseModels;
using RestApi.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestApi.Controllers
{
    [ApiController]
    [Route("todos")]
    public class TodosController : ControllerBase
    {
        private readonly ITodoRepository _todoRepository;
        private readonly AppSettings _appSettings;

        public TodosController(ITodoRepository todoRepository, IOptions<AppSettings> appSettings)
        {
            _todoRepository = todoRepository;
            _appSettings = appSettings.Value;
        }

        [HttpGet]
        [ApiKey]
        public async Task<IEnumerable<TodoResponse>> GetTodos()
        {
            var userId = (Guid) HttpContext.Items["userId"];
            var todos = await _todoRepository.GetAllAsync(userId);

            return todos.Select(todo => todo.MapToTodoResponseFromTodoRead());
        }

        [HttpGet]
        [ApiKey]
        [Route("{id}")]
        public async Task<ActionResult<TodoResponse>> GetTodo(Guid id)
        {
            var releaseDate = _appSettings.RealeaseDate;

            var currentDate = DateTime.Now;

            if (releaseDate >= currentDate)
            {
                return BadRequest("Feature not released");
            }

            var userId = (Guid)HttpContext.Items["userId"];
            var todo = await _todoRepository.GetAsync(id, userId);

            if (todo is null)
            {
                return NotFound($"Todo item with id: {id} does not exist");
            }

            return todo.MapToTodoResponseFromTodoRead();
        }

        [HttpPost]
        [ApiKey]
        public async Task<ActionResult<TodoResponse>> AddTodo([FromBody] AddTodoRequest request)
        {
            var userId = (Guid)HttpContext.Items["userId"];

            var todo = new TodoWrite
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                Description = request.Description,
                Difficulty = request.Difficulty,
                DateCreated = DateTime.Now,
                IsDone = false,
                UserId = userId
            };

            await _todoRepository.SaveOrUpdateAsync(todo);

            return CreatedAtAction(nameof(GetTodo), new { id = todo.Id }, todo.MapToTodoResponseFromTodoWrite());
        }

        [HttpPut]
        [ApiKey]
        [Route("{id}")]
        public async Task<ActionResult<TodoResponse>> UpdateTodo(Guid id, [FromBody] SaveOrUpdateTodoRequest request)
        {
            if (request is null)
            {
                return BadRequest();
            }

            var userId = (Guid)HttpContext.Items["userId"];
            var todoToUpdate = await _todoRepository.GetAsync(id, userId);

            if (todoToUpdate is null)
            {
                return NotFound($"Todo item with id: {id} does not exist");
            }

            var UpdatedTodo = new TodoWrite
            {
                Id = id,
                Title = request.Title,
                Description = request.Description,
                Difficulty = request.Difficulty,
                DateCreated = todoToUpdate.DateCreated,
                IsDone = request.IsDone,
                UserId = todoToUpdate.UserId
            };

            await _todoRepository.SaveOrUpdateAsync(UpdatedTodo);

            return UpdatedTodo.MapToTodoResponseFromTodoWrite();
        }

        [HttpPut]
        [ApiKey]
        [Route("{id}/status")]
        public async Task<ActionResult<TodoResponse>> UpdateStatus(Guid id, [FromBody] UpdateStatusRequest request)
        {
            if (request is null)
            {
                return BadRequest();
            }

            var userId = (Guid)HttpContext.Items["userId"];
            var todoToUpdate = await _todoRepository.GetAsync(id, userId);

            if (todoToUpdate is null)
            {
                return NotFound($"Todo item with id: {id} does not exist");
            }

            var UpdatedTodo = new TodoWrite
            {
                Id = id,
                Title = todoToUpdate.Title,
                Description = todoToUpdate.Description,
                Difficulty = todoToUpdate.Difficulty,
                DateCreated = todoToUpdate.DateCreated,
                IsDone = request.IsDone,
                UserId = todoToUpdate.UserId
            };

            await _todoRepository.SaveOrUpdateAsync(UpdatedTodo);

            return UpdatedTodo.MapToTodoResponseFromTodoWrite();
        }

        [HttpDelete]
        [ApiKey]
        [Route("{id}")]
        public async Task<ActionResult> DeleteComment(Guid id)
        {
            var userId = (Guid)HttpContext.Items["userId"];
            var todoDelete = await _todoRepository.GetAsync(id, userId);

            if (todoDelete is null)
            {
                return NotFound($"Todo item with id: {id} does not exist");
            }

            await _todoRepository.DeleteAsync(id);

            return NoContent();
        }
    }
}
