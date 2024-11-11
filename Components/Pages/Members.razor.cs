using Microsoft.AspNetCore.Components;
using MudBlazor;
using Szakdolgozat.Interfaces;

namespace Szakdolgozat.Components.Pages
{
    public partial class Members
    {
        [Inject]
        private IMembersService _membersService { get; set; }
        [Inject]
        private IMembershipService _membershipService { get; set; }
        [Inject]
        private IPurchasesService _purchasesService { get; set; }

        private List<Data.Models.Members> MembersList = new List<Data.Models.Members>();
        private List<Data.Models.Membership> MembershipList = new List<Data.Models.Membership>();
        private List<Data.Models.Purchases> PurchaseList = new List<Data.Models.Purchases>();

        private Data.Models.Members member = new Data.Models.Members();
        private Data.Models.Purchases? purchase = new Data.Models.Purchases();
        private string? _searchString;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            MembersList = await GetMembers();
            MembershipList = await _membershipService.GetAllMemberships();
            PurchaseList = await _purchasesService.GetAllPurchases();
        }

        private Func<Data.Models.Members, bool> _quickFilter => x =>
        {
            if (string.IsNullOrWhiteSpace(_searchString))
                return true;

            if (x.Name.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;

            if (x.Email.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;

            return false;
        };
        public async Task<List<Data.Models.Members>> GetMembers() => await _membersService.GetAllMembers();

        private bool GetMemberStatus(Guid id)
        {
            Data.Models.Purchases? purchase = new Data.Models.Purchases();

            if (id == Guid.Empty)
            {
                return false;
            }
            else
            {
                purchase = PurchaseList
                .Where(p => p.BuyerId == id)
                .OrderByDescending(p => p.CreatedDate)
                .FirstOrDefault();
                if(purchase != null)
                {
                    int? days = MembershipList.Where(p => p.Id == purchase.PassId).Select(p => p.ValidDays).FirstOrDefault();
                    if (days != null && purchase.CreatedDate.AddDays(days.Value) > DateTime.Today)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

    void CommittedItemChanges(Data.Models.Members item)
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

        private async Task OnBirthdayChanged(Data.Models.Members member, DateTime? newDate)
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

        void HandleValidSubmit(Data.Models.Members item)
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
        void DeleteMember(Data.Models.Members member)
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
