using Szakdolgozat.Data.Models;

namespace Szakdolgozat.Interfaces
{
    public interface ITodoService
    {
        public Task CreateTodo(Todo todo);
        public Task<Todo?> GetTodoById(Guid id);
        public Task<List<Todo>> GetAllTodos();
        public Task UpdateTodo(Todo updatedTodo);
        public Task DeleteTodo(Guid id);
    }
}
