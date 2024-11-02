using Microsoft.AspNetCore.Components;
using MudBlazor;
using Szakdolgozat.Interfaces;
using Szakdolgozat.Services;

namespace Szakdolgozat.Components.Pages
{
    public partial class Todo
    {
        [Inject]
        private ITodoService _todoService { get; set; }

        private List<Data.Models.Todo> TodoList = new List<Data.Models.Todo>();

        private Data.Models.Todo todo = new Data.Models.Todo();
        private string? _searchString;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            TodoList = await GetTodos();
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
                item.Id = Guid.NewGuid();
                item.CreatedDate = DateTime.Now;
                
                _todoService.CreateTodo(item);
                TodoList.Add(item);
                todo = new Data.Models.Todo();
                StateHasChanged();
                Snackbar.Add($"Új feladat sikeresen hozzáadva!", Severity.Success, c => c.SnackbarVariant = Variant.Outlined);
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
