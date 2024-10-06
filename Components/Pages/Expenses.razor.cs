using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using MudBlazor;
using System.Security.Claims;
using Szakdolgozat.Data;
using Szakdolgozat.Interfaces;
using Szakdolgozat.Services;

namespace Szakdolgozat.Components.Pages
{
    public partial class Expenses
    {
        [Inject]
        IExpensesService _expensesService { get; set; }
        [Inject]
        public UserManager<ApplicationUser> UserManager { get; set; }

        [Inject]
        private AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        private List<Szakdolgozat.Data.Models.Expenses> ExpensesList = new List<Szakdolgozat.Data.Models.Expenses>();
        private Szakdolgozat.Data.Models.Expenses expense = new Szakdolgozat.Data.Models.Expenses();
        private List<ApplicationUser> Users = new List<ApplicationUser>();
        private string? _searchString;
        private string? userId;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            ExpensesList = await GetExpenses();
            Users = UserManager.Users.ToList();
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            if (user.Identity.IsAuthenticated)
            {
                userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            }
        }

        public async Task<List<Szakdolgozat.Data.Models.Expenses>> GetExpenses() => await _expensesService.GetAllExpenses();

        public string GetUsernameById(string id)
        {
            var user = Users.FirstOrDefault(u => u.Id == id);
            if (user != null)
            {
                return user?.UserName;
            }
            else
            {
                return "Ismeretlen Felhasználó";
            }
        }

        void CommittedItemChanges(Szakdolgozat.Data.Models.Expenses item)
        {
            try
            {
                _expensesService.UpdateExpense(item);
                Snackbar.Add($"Kiadáshoz tartozó információ sikeresen frissítve", Severity.Success, c => c.SnackbarVariant = Variant.Outlined);
            }
            catch (Exception e)
            {
                Snackbar.Add($"HIBA! {e}", Severity.Error, c => c.SnackbarVariant = Variant.Outlined);
                throw;
            }
        }

        void HandleValidSubmit(Szakdolgozat.Data.Models.Expenses item)
        {
            try
            {
                if (userId == null)
                {
                    Snackbar.Add($"HIBA! Csak érvényes felhasználók tudnak költséget hozzá adni!", Severity.Error, c => c.SnackbarVariant = Variant.Outlined);
                }
                else
                {
                    item.Id = Guid.NewGuid();
                    item.CreatorId = userId;
                    _expensesService.CreateExpense(item);
                    ExpensesList.Add(item);
                    expense = new Data.Models.Expenses();
                    StateHasChanged();
                    Snackbar.Add($"Új kiadás sikeresen hozzáadva!", Severity.Success, c => c.SnackbarVariant = Variant.Outlined);
                }
            }
            catch (Exception e)
            {
                Snackbar.Add($"HIBA! {e}", Severity.Error, c => c.SnackbarVariant = Variant.Outlined);
                throw;
            }
        }

        private async Task OnCreatedDateChanged(Szakdolgozat.Data.Models.Expenses expense, DateTime? newDate)
        {
            if (newDate.HasValue)
            {
                try
                {
                    expense.CreatedDate = newDate.Value;
                    await _expensesService.UpdateExpense(expense);
                    Snackbar.Add($"Kiadáshoz tartozó dátum sikeresen frissítve.", Severity.Success, c => c.SnackbarVariant = Variant.Outlined);
                }
                catch (Exception e)
                {
                    Snackbar.Add($"HIBA! {e}", Severity.Error, c => c.SnackbarVariant = Variant.Outlined);
                    throw;
                }
            }
        }

        void DeleteExpense(Szakdolgozat.Data.Models.Expenses expense)
        {
            try
            {
                _expensesService.DeleteExpense(expense.Id);
                ExpensesList.Remove(expense);
                StateHasChanged();
                Snackbar.Add($"A kiválasztott kiadás sikeresen törölve!", Severity.Success, c => c.SnackbarVariant = Variant.Outlined);
            }
            catch (Exception e)
            {
                Snackbar.Add($"HIBA! {e}", Severity.Error, c => c.SnackbarVariant = Variant.Outlined);
                throw;
            }
        }

        private Func<Szakdolgozat.Data.Models.Expenses, bool> _quickFilter => x =>
        {
            if (string.IsNullOrWhiteSpace(_searchString))
                return true;

            if (x.Name.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;

            return false;
        };
    }
}
