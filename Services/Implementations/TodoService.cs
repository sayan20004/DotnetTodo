using MongoDB.Driver;
using TodoDotNet.Models;
using TodoDotNet.Services.Interfaces;

namespace TodoDotNet.Services.Implementations
{
    public class TodoService : ITodoService
    {
        private readonly IMongoCollection<Todo> _todos;

        public TodoService(IMongoDatabase database)
        {
            _todos = database.GetCollection<Todo>("Todos");
        }

        public async Task<Todo> CreateAsync(Todo todo)
        {
            todo.Id = Guid.NewGuid();
            todo.CreatedAt = DateTime.UtcNow;
            todo.UpdatedAt = DateTime.UtcNow;

            await _todos.InsertOneAsync(todo);
            return todo;
        }

        public async Task<List<Todo>> GetAllAsync()
        {
            return await _todos.Find(_ => true).ToListAsync();
        }

        public async Task<Todo?> GetByIdAsync(Guid id)
        {
            return await _todos.Find(t => t.Id == id).FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateAsync(Guid id, Todo updatedTodo)
        {
            var existingTodo = await _todos.Find(t => t.Id == id).FirstOrDefaultAsync();
            if (existingTodo == null)
                return false;

            updatedTodo.Id = id;
            updatedTodo.CreatedAt = existingTodo.CreatedAt;
            updatedTodo.UpdatedAt = DateTime.UtcNow;

            var result = await _todos.ReplaceOneAsync(t => t.Id == id, updatedTodo);
            return result.MatchedCount > 0;
        }


        public async Task<bool> DeleteAsync(Guid id)
        {
            var result = await _todos.DeleteOneAsync(t => t.Id == id);
            return result.DeletedCount > 0;
        }
    }
}
