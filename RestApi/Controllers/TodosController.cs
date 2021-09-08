﻿using Microsoft.AspNetCore.Mvc;
using Persistence.Models.WriteModels;
using Persistence.Repositories;
using RestApi.Models;
using RestApi.Models.RequestModels;
using RestApi.Models.ResponseModels;
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

        public TodosController(ITodoRepository todoRepository)
        {
            _todoRepository = todoRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<TodoResponse>> GetTodos()
        {
            var todos = await _todoRepository.GetAllAsync();

            return todos.Select(todo => todo.MapToTodoResponseFromTodoRead());
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<TodoResponse>> GetTodo(Guid id)
        {
            var todo = await _todoRepository.GetAsync(id);

            if (todo is null)
            {
                return NotFound($"Todo item with id: {id} does not exist");
            }

            return todo.MapToTodoResponseFromTodoRead();
        }

        [HttpPost]
        public async Task<ActionResult<TodoResponse>> AddTodo([FromBody] AddTodoRequest request)
        {
            var todo = new TodoWrite
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                Description = request.Description,
                Difficulty = request.Difficulty,
                DateCreated = DateTime.Now,
                IsDone = false
            };

            await _todoRepository.SaveOrUpdateAsync(todo);

            return CreatedAtAction(nameof(GetTodo), new { id = todo.Id }, todo.MapToTodoResponseFromTodoWrite());
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<TodoResponse>> UpdateTodo(Guid id, [FromBody] SaveOrUpdateTodoRequest request)
        {
            if (request is null)
            {
                return BadRequest();
            }

            var todoToUpdate = await _todoRepository.GetAsync(id);

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
                IsDone = request.IsDone
            };

            await _todoRepository.SaveOrUpdateAsync(UpdatedTodo);

            return UpdatedTodo.MapToTodoResponseFromTodoWrite();
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> DeleteComment(Guid id)
        {
            var todoDelete = await _todoRepository.GetAsync(id);

            if (todoDelete is null)
            {
                return NotFound();
            }

            await _todoRepository.DeleteAsync(id);

            return NoContent();
        }
    }
}
