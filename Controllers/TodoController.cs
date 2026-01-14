using Microsoft.AspNetCore.Mvc;
using TodoDotNet.Models;
using TodoDotNet.Services.Interfaces;

namespace TodoDotNet.Controllers
{
    [ApiController]
    [Route("api/todos")]
    public class TodoController : ControllerBase
    {
        private readonly ITodoService _todoService;

        public TodoController(ITodoService todoService)
        {
            _todoService = todoService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Todo todo)
        {
            if (todo == null || string.IsNullOrWhiteSpace(todo.Title))
                return BadRequest("Title is required.");

            var createdTodo = await _todoService.CreateAsync(todo);
            return CreatedAtAction(nameof(GetById), new { id = createdTodo.Id }, createdTodo);
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var todos = await _todoService.GetAllAsync();
            return Ok(todos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var todo = await _todoService.GetByIdAsync(id);

            if (todo == null)
                return NotFound();

            return Ok(todo);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] Todo todo)
        {
            if (todo == null || string.IsNullOrWhiteSpace(todo.Title))
                return BadRequest("Title is required.");

            todo.Id = id;

            var updated = await _todoService.UpdateAsync(id, todo);

            if (!updated)
                return NotFound();

            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _todoService.DeleteAsync(id);

            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
