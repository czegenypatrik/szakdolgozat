using Szakdolgozat.Data;
using Szakdolgozat.Data.Models;
using Szakdolgozat.Interfaces;

namespace Szakdolgozat.Services
{
    public class ExpensesService : IExpensesService
    {
        private readonly ApplicationDbContext _context;

        public ExpensesService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateExpense(Expenses expense)
        {
            expense.Id = Guid.NewGuid();
            _context.Expenses.Add(expense);
            await _context.SaveChangesAsync();
        }

        public async Task<Expenses?> GetExpenseById(Guid id)
        {
            return await _context.Expenses.FindAsync(id);
        }

        public async Task<List<Expenses>> GetAllExpenses()
        {
            return _context.Expenses.ToList();
        }

        public async Task UpdateExpense(Expenses updatedExpense)
        {

            var expense = await _context.Expenses.FindAsync(updatedExpense.Id);
            if (expense != null)
            {
                expense.Name = updatedExpense.Name;
                expense.Price = updatedExpense.Price;
                expense.CreatorId = updatedExpense.CreatorId;
                expense.CreatedDate = updatedExpense.CreatedDate;

                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteExpense(Guid id)
        {

            var expense = await _context.Expenses.FindAsync(id);
            if (expense != null)
            {
                _context.Expenses.Remove(expense);
                await _context.SaveChangesAsync();
            }
        }
    }
}
