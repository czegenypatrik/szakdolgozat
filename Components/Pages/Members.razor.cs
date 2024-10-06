using Microsoft.AspNetCore.Components;
using MudBlazor;
using Szakdolgozat.Interfaces;

namespace Szakdolgozat.Components.Pages
{
    public partial class Members
    {
        [Inject]
        private IMembersService _membersService { get; set; }

        private List<Szakdolgozat.Data.Models.Members> MembersList = new List<Szakdolgozat.Data.Models.Members>();

        private Szakdolgozat.Data.Models.Members member = new Szakdolgozat.Data.Models.Members();
        private string? _searchString;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            MembersList = await GetMembers();
        }

        private Func<Szakdolgozat.Data.Models.Members, bool> _quickFilter => x =>
        {
            if (string.IsNullOrWhiteSpace(_searchString))
                return true;

            if (x.Name.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;

            if (x.Email.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;

            return false;
        };
        public async Task<List<Szakdolgozat.Data.Models.Members>> GetMembers() => await _membersService.GetAllMembers();

        void CommittedItemChanges(Szakdolgozat.Data.Models.Members item)
        {
            try
            {
                _membersService.UpdateMember(item);
                Snackbar.Add($"Taghoz tartozó információ sikeresen frissítve.", Severity.Success, c => c.SnackbarVariant = Variant.Outlined);
            }
            catch (Exception e)
            {
                Snackbar.Add($"HIBA! {e}", Severity.Error, c => c.SnackbarVariant = Variant.Outlined);
                throw;
            }
        }

        private async Task OnBirthdayChanged(Szakdolgozat.Data.Models.Members member, DateTime? newDate)
        {
            if (newDate.HasValue)
            {
                try
                {
                    member.Birthday = newDate.Value;
                    await _membersService.UpdateMember(member);
                    Snackbar.Add($"Taghoz tartozó születésnap sikeresen frissítve.", Severity.Success, c => c.SnackbarVariant = Variant.Outlined);
                }
                catch (Exception e)
                {
                    Snackbar.Add($"HIBA! {e}", Severity.Error, c => c.SnackbarVariant = Variant.Outlined);
                    throw;
                }
            }
        }

        void HandleValidSubmit(Szakdolgozat.Data.Models.Members item)
        {
            try
            {
                item.JoinedDate = DateTime.Now;
                _membersService.CreateMember(item);
                MembersList.Add(item);
                member = new Data.Models.Members();
                StateHasChanged();
                Snackbar.Add($"Új tag sikeresen hozzáadva!", Severity.Success, c => c.SnackbarVariant = Variant.Outlined);
            }
            catch (Exception e)
            {
                Snackbar.Add($"HIBA! {e}", Severity.Error, c => c.SnackbarVariant = Variant.Outlined);
                throw;
            }
        }
        void DeleteMember(Szakdolgozat.Data.Models.Members member)
        {
            try
            {
                _membersService.DeleteMembers(member.Id);
                MembersList.Remove(member);
                StateHasChanged();
                Snackbar.Add($"A kiválasztott tag sikeresen törölve!", Severity.Success, c => c.SnackbarVariant = Variant.Outlined);
            }
            catch (Exception e)
            {
                Snackbar.Add($"HIBA! {e}", Severity.Error, c => c.SnackbarVariant = Variant.Outlined);
                throw;
            }
        }
    }
}
