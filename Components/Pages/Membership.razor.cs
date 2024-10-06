using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using Szakdolgozat.Interfaces;
using Szakdolgozat.Services;
using Szakdolgozat.Data;
using static MudBlazor.CategoryTypes;
using Szakdolgozat.Data.Models;
using MudBlazor;

namespace Szakdolgozat.Components.Pages
{
    public partial class Membership
    {
        [Inject]
        private IMembershipService _membershipService { get; set; }

        private List<Szakdolgozat.Data.Models.Membership> Memberships = new List<Szakdolgozat.Data.Models.Membership>();

        private Szakdolgozat.Data.Models.Membership membership = new Szakdolgozat.Data.Models.Membership();
        private string? _searchString;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Memberships = await GetMemberships();
        }

        private Func<Szakdolgozat.Data.Models.Membership, bool> _quickFilter => x =>
        {
            if (string.IsNullOrWhiteSpace(_searchString))
                return true;

            if (x.Type.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;

            if (x.Description.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;

            return false;
        };
        public async Task<List<Szakdolgozat.Data.Models.Membership>> GetMemberships() => await _membershipService.GetAllMemberships();

        void CommittedItemChanges(Szakdolgozat.Data.Models.Membership item)
        {
            _membershipService.UpdateMembership(item);
            Snackbar.Add($"Bérlet információ sikeresen frissítve", Severity.Success, c => c.SnackbarVariant = Variant.Outlined);
        }

        void HandleValidSubmit(Szakdolgozat.Data.Models.Membership item)
        {
            try
            {
                _membershipService.CreateMembership(item);
                Memberships.Add(item);
                membership = new Data.Models.Membership();
                StateHasChanged();
                Snackbar.Add($"Új bérlettípus sikeresen hozzáadva!", Severity.Success, c => c.SnackbarVariant = Variant.Outlined);
            }
            catch (Exception e)
            {
                Snackbar.Add($"HIBA! {e}", Severity.Error, c => c.SnackbarVariant = Variant.Outlined);
                throw;
            }
        }
        void DeleteMembership(Szakdolgozat.Data.Models.Membership membership)
        {
            try
            {
                _membershipService.DeleteMembership(membership.Id);
                Memberships.Remove(membership);
                StateHasChanged();
                Snackbar.Add($"A kiválasztott bérlet típus sikeresen törölve!", Severity.Success, c => c.SnackbarVariant = Variant.Outlined);
            }
            catch (Exception e)
            {
                Snackbar.Add($"HIBA! {e}", Severity.Error, c => c.SnackbarVariant = Variant.Outlined);
                throw;
            }
        }
    }

}
