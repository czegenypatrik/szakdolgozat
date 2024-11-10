using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using MudBlazor;
using Newtonsoft.Json.Linq;
using System.Security.Claims;
using Szakdolgozat.Data;
using Szakdolgozat.Interfaces;
using Szakdolgozat.Services;

namespace Szakdolgozat.Components.Pages
{
    public partial class Todo
    {
        [Inject]
        private ITodoService _todoService { get; set; }
        [Inject]
        public UserManager<ApplicationUser> UserManager { get; set; }

        [Inject]
        private AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        private List<Data.Models.Todo> TodoList = new List<Data.Models.Todo>();
        private List<ApplicationUser> Users = new List<ApplicationUser>();

        private Data.Models.Todo todo = new Data.Models.Todo();
        private string? _searchString;
        private string? userId;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            TodoList = await GetTodos();
            Users = UserManager.Users.ToList();
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            if (user.Identity.IsAuthenticated)
            {
                userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            }
        }

        public async Task<List<Data.Models.Todo>> GetTodos() => await _todoService.GetAllTodos();

        private Func<Data.Models.Todo, bool> _quickFilter => x =>
        {
            if (string.IsNullOrWhiteSpace(_searchString))
                return true;

            if (x.Name.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;

            if (x.Description.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;

            return false;
        };

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

        void CommittedItemChanges(Data.Models.Todo item)
        {
            try
            {
                _todoService.UpdateTodo(item);
                Snackbar.Add($"Feladathoz tartozó adatok sikeresen frissítve!", Severity.Success, c => c.SnackbarVariant = Variant.Outlined);
            }
            catch (Exception e)
            {
                Snackbar.Add($"HIBA! {e}", Severity.Error, c => c.SnackbarVariant = Variant.Outlined);
                throw;
            }
        }

        void HandleValidSubmit(Data.Models.Todo item)
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
                    item.CreatedDate = DateTime.Now;
                    _todoService.CreateTodo(item);
                    TodoList.Add(item);
                    todo = new Data.Models.Todo();
                    StateHasChanged();
                    Snackbar.Add($"Új feladat sikeresen hozzáadva!", Severity.Success, c => c.SnackbarVariant = Variant.Outlined);
                }
                
            }
            catch (Exception e)
            {
                Snackbar.Add($"HIBA! {e}", Severity.Error, c => c.SnackbarVariant = Variant.Outlined);
                throw;
            }
        }

        void DeleteTodo(Data.Models.Todo todo)
        {
            try
            {
                _todoService.DeleteTodo(todo.Id);
                TodoList.Remove(todo);
                StateHasChanged();
                Snackbar.Add($"A kiválasztott feladat sikeresen törölve!", Severity.Success, c => c.SnackbarVariant = Variant.Outlined);
            }
            catch (Exception e)
            {
                Snackbar.Add($"HIBA! {e}", Severity.Error, c => c.SnackbarVariant = Variant.Outlined);
                throw;
            }
        }
    }
}
