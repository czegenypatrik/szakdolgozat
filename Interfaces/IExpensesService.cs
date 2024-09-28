using Szakdolgozat.Data.Models;

namespace Szakdolgozat.Interfaces
{
    public interface IExpensesService
    {
        public Task CreateExpense(Expenses expense);
        public Task<Expenses?> GetExpenseById(Guid id);
        public Task<List<Expenses>> GetAllExpenses();
        public Task UpdateExpense(Expenses updatedExpense);
        public Task DeleteExpense(Guid id);
    }
}
