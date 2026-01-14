using TodoDotNet.Models;

namespace TodoDotNet.Services.Interfaces
{
    public interface ITodoService
    {
        Task<Todo> CreateAsync(Todo todo);

        Task<List<Todo>> GetAllAsync();

        Task<Todo?> GetByIdAsync(Guid id);

        Task<bool> UpdateAsync(Guid id, Todo updatedTodo);

        Task<bool> DeleteAsync(Guid id);
    }
}
