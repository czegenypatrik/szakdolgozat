using Szakdolgozat.Data.Models;
using Szakdolgozat.Data;
using Szakdolgozat.Interfaces;

namespace Szakdolgozat.Services
{
    public class TodoService : ITodoService
    {
        private readonly ApplicationDbContext _context;

        public TodoService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateTodo(Todo todo)
        {
            todo.Id = Guid.NewGuid();
            _context.Todo.Add(todo);
            await _context.SaveChangesAsync();
        }

        public async Task<Todo?> GetTodoById(Guid id)
        {
            return await _context.Todo.FindAsync(id);
        }

        public async Task<List<Todo>> GetAllTodos()
        {
            return _context.Todo.ToList();
        }

        public async Task UpdateTodo(Todo updatedTodo)
        {

            var todo = await _context.Todo.FindAsync(updatedTodo.Id);
            if (todo != null)
            {
                todo.Name = updatedTodo.Name;
                todo.Description = updatedTodo.Description;
                todo.CreatedDate = updatedTodo.CreatedDate;
                todo.DueDate = updatedTodo.DueDate;
                todo.CreatorId = updatedTodo.CreatorId;
                todo.Severity = updatedTodo.Severity;
                todo.Status = updatedTodo.Status;

                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteTodo(Guid id)
        {

            var todo = await _context.Todo.FindAsync(id);
            if (todo != null)
            {
                _context.Todo.Remove(todo);
                await _context.SaveChangesAsync();
            }
        }
    }
}
